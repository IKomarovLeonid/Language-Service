import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {ApiClient} from "../../services/api.client";
import {
  CreateGameResultRequestModel, GameAttemptModel, UserModel,
  WordGameResultModel,
  WordLanguageType,
  WordModel,
  WordStatisticsModel,
} from "../../shared/main.api";

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit {
  userInput: string = '';
  feedback: string = '';
  attempts: number = 0;
  correctCount: number = 0;
  maxStreak: number = 0;

  // Game settings
  isRepetitionsAllowed: boolean = false;
  enableTimer: boolean = false;
  playSounds: boolean = true;
  showTranslations: boolean = true;

  // words
  private words: WordModel[] = [];
  private filteredWords: WordModel[] = [];
  // stats
  private statistics: WordStatisticsModel[] = [];
  public currentStats: WordStatisticsModel | undefined = undefined;
  // games
  private games: GameAttemptModel[] = [];
  // temporary
  private userId = 1;
  private user: UserModel | undefined;

  word: WordModel | undefined;
  // filters
  allowedFilters: Set<string> = new Set<string>();
  selectedFilters: Set<string> = new Set<string>();
  // data
  private gameProgressData: Map<number, { correctCount: number, totalCount: number }> = new Map();
  private recordedIndex = 0;

  constructor(private api: ApiClient) { }

  checkAnswer() {
    this.resetMessage();
    this.attempts++;
    let answer = this.userInput.toLowerCase();
    let filtered = this.word!!.translations?.filter(w => answer === w.toLowerCase());
    if(filtered && filtered.length > 0){
      console.log(this.word);
      this.saveAttemptData(true, this.word?.id!!);
      this.feedback = 'Correct!';
      this.correctCount++;
      this.maxStreak ++;
      this.setAnyWord();
    }else {
      this.saveAttemptData(false, this.word?.id!!);
      this.maxStreak = 0;
      this.feedback = 'Incorrect, try again! Correct is: ' + this.word?.translations?.join(',') + "";
    }
    this.resetUserInput();
  }

  private saveAttemptData(isCorrect: boolean, wordId: number){
    if (this.gameProgressData.has(wordId)) {
      const existingStats = this.gameProgressData.get(wordId)!;
      if(isCorrect){
        existingStats.correctCount += 1;
        existingStats.totalCount += 1;
      } else{
        existingStats.totalCount += 1;
      }
    } else {
      let correctCount = isCorrect ? 1 : 0;
      let totalCount = 1;
      this.gameProgressData.set(wordId, { correctCount, totalCount });
    }
  }

  get successRate(): number {
    return this.attempts === 0 ? 0 : (this.correctCount / this.attempts) * 100;
  }

  async finishGame() {
    if(this.gameProgressData.size === 0) return;
    let request = new CreateGameResultRequestModel();
    const results: WordGameResultModel[] = [];
    this.gameProgressData.forEach((stats, wordId) => {
      let result = new WordGameResultModel();
      result.wordId = wordId;
      result.correctCount = stats.correctCount;
      result.totalCount = stats.totalCount;
      results.push(result);
    });
    request.results = results;
    request.userId = 1;
    request.maxStreak = this.maxStreak;
    await this.api.createGameResult(request);
    this.resetGame();
    this.gameProgressData = new Map();
    await this.loadStatistics();
    this.setAnyWord();
    await this.loadUserProfile(this.userId);
  }

  resetGame() {
    this.attempts = 0;
    this.correctCount = 0;
    this.maxStreak = 0;
    this.resetUserInput();
    this.resetMessage();
    this.setAnyWord();
  }

  resetUserInput(){
    this.userInput = '';
  }

  resetMessage(){
    this.feedback = '';
  }

  async ngOnInit(): Promise<void> {
    await this.api.getWords().then(model => {
      this.words = model?.items!!;
      for (let i = 0; i < this.words.length; i++) {
        let attribute = this.words[i].attributes;
        if(attribute){
          attribute.split(',').map(item => item.trim()).forEach(item => this.allowedFilters.add(item));
        }
      }
    }).catch(error => {
      alert("Failed to retrieve words");
    });
    await this.loadUserProfile(this.userId);
    await this.loadStatistics();
    await this.loadUserGames(this.userId);
    this.filterWords(Array.from(this.selectedFilters), WordLanguageType.SpanishRussian);
    this.setAnyWord();
  }

  private filterWords(filters: string[]| undefined, language: WordLanguageType){
    if(filters === undefined || filters.length < 1) {
      this.filteredWords = this.words.filter(w => w.languageType === language);
      return;
    }
    let byLanguage = this.words.filter(w => w.languageType === language);
    this.filteredWords = byLanguage.filter(item => {
      if (!item.attributes) {
        return false;
      }

      const attributeSet = new Set(item.attributes.split(',').map(attr => attr.trim()));

      return filters.every(filter => attributeSet.has(filter));
    });
  }

  setAnyWord(){
    console.log(this.recordedIndex);
    if(this.filteredWords.length == 0) {
      this.word = undefined;
      return;
    }
    if(this.isRepetitionsAllowed){
      const randomIndex = Math.floor(Math.random() * this.getFilteredWordsCount());
      this.word = this.filteredWords[randomIndex];
    }
    else {
      if(this.recordedIndex < this.getFilteredWordsCount() - 1){
        let newIndex = this.recordedIndex + 1;
        this.word = this.filteredWords[newIndex];
      } else {
        this.recordedIndex = 0;
        this.word = this.filteredWords[this.recordedIndex];
      }
    }
    this.setWordStatistics();
  }

  toggleFilterSelection(filter: string) {
      if (this.selectedFilters.has(filter)) {
        this.selectedFilters.delete(filter);
      } else {
        this.selectedFilters.add(filter);
      }
      this.filterWords(Array.from(this.selectedFilters), WordLanguageType.SpanishRussian);
      this.setAnyWord();
  }

  private setWordStatistics(){
    if(!this.word) return;
    let id = this.word?.id;
    let stats= this.statistics.filter(s => s.wordId === id && s.userId === this.userId);
    if(stats.length == 0) {
      this.currentStats = undefined;
      return;
    }
    this.currentStats = stats[0];
    console.log(this.currentStats);
  }

  private async loadStatistics(){
    await this.api.getStatistics(this.userId).then(model => {
      this.statistics = model?.items!!;
    }).catch(error => {
      alert("Failed to retrieve statistics");
    });
  }

  private async loadUserGames(userId: number){
    await this.api.games.getGames(userId).then(model => {
      this.games = model?.items!!;
    }).catch(error => {
      alert("Failed to retrieve user games");
    });
  }

  private async loadUserProfile(userId: number){
    await this.api.users.getUsers(userId).then(model => {
      this.user = model.find(u => u.id === userId);
    }).catch(error => {
      alert("Failed to retrieve user");
    });
  }

  public getTotalWordsCount(){
    return this.words.length;
  }

  public getFilteredWordsCount(){
    return this.filteredWords.length;
  }

  public getTotalUserGamesCount(){
    return this.games.length;
  }

  public getAllTimeUserRate(){
    return this.user ? this.user.allTimeSuccessRate : 0;
  }

  public getAllTimeMaxStreak(){
    return this.user ? this.user.maxStreak : 0;
  }

  public setRepetitionsToggle(){
    this.isRepetitionsAllowed = !this.isRepetitionsAllowed;
  }

}
