import {Component, OnInit} from '@angular/core';
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
  isRepetitionsAllowed: boolean = true;
  hardMode: boolean = false;

  // last index repetitions
  lastIndex = 0;

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
  // soft mode
  possibleAnswers: Set<string> = new Set<string>();
  // data
  private gameProgressData: Map<number, { correctCount: number, totalCount: number }> = new Map();

  constructor(private api: ApiClient) { }

  checkAnswer() {
    this.resetMessage();
    this.attempts++;
    let answer = this.userInput.toLowerCase();
    let filtered = this.word!!.translations?.filter(w => answer === w.toLowerCase());
    if(filtered && filtered.length > 0){
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
    request.userId = this.userId;
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
    if(this.filteredWords.length == 0) {
      this.word = undefined;
      this.possibleAnswers.clear();
      return;
    }
    let index;
    if(this.isRepetitionsAllowed){
      index = Math.floor(Math.random() * this.getFilteredWordsCount());
    } else{
      let len = this.getFilteredWordsCount();
      if(this.lastIndex >= len){
        this.lastIndex = 0;
      }
      index = this.lastIndex;
      this.lastIndex ++;
    }

    this.word = this.filteredWords[index];
    this.setWordStatistics();
    if(this.hardMode){
      this.possibleAnswers.clear();
      if(this.word.translations){
        this.possibleAnswers.add(this.word.translations[0]);
      }
      // fill allowed answers;
      // @ts-ignore
      let wordsFiltered = this.filteredWords.filter(w => w.id != this.word?.id && w.attributes?.includes(this.word?.attributes?.split(",")[0]));
      if(wordsFiltered.length > 7){
        let indexes = this.getThreeRandomIndexes(wordsFiltered);
        for(let i = 0; i < indexes.length; i++){
          // @ts-ignore
          this.possibleAnswers.add(wordsFiltered[indexes[i]].translations[0]);
        }
      }
      this.possibleAnswers = this.shuffleSet(this.possibleAnswers);
    }
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
    await this.api.getUser(userId).then(model => {
      this.user = model!!.find(u => u.id === userId);
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
    this.setAnyWord();
  }

  public checkPossibleAnswer(answer: string){
    this.resetMessage();
    this.attempts++;
    let filtered = this.word!!.translations?.filter(w => answer === w.toLowerCase());
    if(filtered && filtered.length > 0){
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

  private shuffleSet(inputSet: Set<string>): Set<string> {
    const array = Array.from(inputSet);

    // Fisher-Yates Shuffle
    for (let i = array.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
    }

    return new Set(array);
  }

  getThreeRandomIndexes<T>(array: T[]): number[] {
    const indexes = Array.from(array.keys()); // [0, 1, 2, ..., array.length - 1]

    // Shuffle using Fisher-Yates
    for (let i = indexes.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [indexes[i], indexes[j]] = [indexes[j], indexes[i]];
    }

    return indexes.slice(0, 5);
  }

  public switchMode(){
    this.hardMode = !this.hardMode;
    this.setAnyWord();
  }

}
