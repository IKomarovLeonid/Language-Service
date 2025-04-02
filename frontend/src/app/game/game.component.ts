import {Component, OnInit} from '@angular/core';
import {ApiClient} from "../../services/api.client";
import {WordLanguageType, WordModel} from "../../shared/main.api";
import {filter} from "rxjs";

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
  totalAttempts: number = 100;
  totalCorrectAnswers: number = 80;
  wordsTranslated: number = 350;
  highestStreak: number = 10;

  // Game settings
  enableHints: boolean = false;
  enableTimer: boolean = false;
  playSounds: boolean = true;
  showTranslations: boolean = true;

  // words
  private words: WordModel[] = [];
  private filteredWords: WordModel[] = [];
  word: WordModel | undefined;
  // filters
  allowedFilters: Set<string> = new Set<string>();
  selectedFilters: Set<string> = new Set<string>();

  constructor(private api: ApiClient) { }

  checkAnswer() {
    this.resetMessage();
    this.attempts++;
    let answer = this.userInput.toLowerCase();
    let filtered = this.word!!.translations?.filter(w => answer === w.toLowerCase());
    if(filtered && filtered.length > 0){
      this.feedback = 'Correct!';
      this.correctCount++;
      this.setAnyWord();
    }else {
      this.feedback = 'Incorrect, try again!';
    }
    this.resetUserInput();
  }

  get successRate(): number {
    return this.attempts === 0 ? 0 : (this.correctCount / this.attempts) * 100;
  }

  get averageSuccessRate(): number {
    return this.totalAttempts === 0 ? 0 : (this.totalCorrectAnswers / this.totalAttempts) * 100;
  }

  finishGame() {
    this.resetGame();
  }

  resetGame() {
    this.attempts = 0;
    this.correctCount = 0;
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
      this.createFilters(this.words);
    }).catch(error => {
      alert("Failed to retrieve words");
    });
    this.filterWords();
    this.setAnyWord();
  }

  private createFilters(words: WordModel[]){
    for (let i = 0; i < words.length; i++) {
      let attribute = words[i].attributes;
      let language = words[i].languageType;
      if(attribute){
        attribute.split(',').map(item => item.trim()).forEach(item => this.allowedFilters.add(item));
      }
    }
    this.filterWords();
  }

  setAnyWord(){
    const randomIndex = Math.floor(Math.random() * this.getFilteredWordsCount());
    this.word = this.filteredWords[randomIndex];
  }

  getFilteredWordsCount(){
    return this.filteredWords.length;
  }

  toggleFilterSelection(filter: string) {
      if (this.selectedFilters.has(filter)) {
        this.selectedFilters.delete(filter);
      } else {
        this.selectedFilters.add(filter);
      }
      this.filterWords();
      this.setAnyWord();
  }

  private filterWords(){
    if(this.selectedFilters.size == 0) {
      console.log("here");
      this.filteredWords = this.words;
      return;
    }
    let byLanguage = this.words.filter(w => w.languageType === WordLanguageType.SpanishRussian);
    console.log("byLanguage", byLanguage.map(w => w.word));
    this.filteredWords = byLanguage.filter(item => {
      if (!item.attributes) {
        return false;
      }
      const attributeSet = new Set(item.attributes.split(',').map(attr => attr.trim()));
      return this.allowedFilters.forEach(filter => attributeSet.has(filter));
    });
    console.log("filteredWords", this.filteredWords.length);
    this.setAnyWord();
  }
}
