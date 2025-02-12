import {Injectable, OnInit} from "@angular/core";
import {WordLanguageType, WordModel} from "../shared/main.api";
import {ApiClient} from "./api.client";
import {BehaviorSubject} from "rxjs";
import {GameStats} from "../app/game.stats";

@Injectable({
  providedIn: 'root'
})
export class GameService{
  private words: WordModel[] = [];
  private filteredWords: WordModel[] = [];
  private allowedFilters: Set<string> = new Set<string>();
  private allowedLanguages: Set<WordLanguageType> = new Set<WordLanguageType>();
  private wordIndex = 0;
  // enable timer if requested
  private isTimerEnabled = false;
  private timer: any;
  private readonly defaultTimerMsc = 10000;
  // cache word
  private _currentWord = new BehaviorSubject<WordModel | null>(null);
  // conjugation
  private isConjugation = false;
  private _isLanguageReversed = new BehaviorSubject<boolean>(false);
  // answers -> for history
  isRepeatWords = true;

  wordErrors = "";

  constructor(private client : ApiClient, private statistics: GameStats) {
    this.loadWords();
  }

  async loadWords(){
    this.allowedFilters.clear();
    let apiResult = await this.client.getWords();
    if(apiResult){
      this.words = apiResult.items!!;
      for (let i = 0; i < this.words.length; i++) {
        let attribute = this.words[i].attributes;
        let language = this.words[i].languageType;
        if(attribute){
          attribute.split(',').map(item => item.trim()).forEach(item => this.allowedFilters.add(item));
        }
        if(language){
          this.allowedLanguages.add(language);
        }
      }
      this.filteredWords = apiResult.items!!.filter(w => w.languageType === WordLanguageType.SpanishRussian);
      this.setAnyWord();
    }
    else alert('Unable to fetch words from server');
  }

  finish(){
    this.statistics.reset();
  }

  getWordsCount() : number{
    return this.filteredWords.length;
  }

  startTimer(): void {
    this.timer = setInterval(() => {
      if(this.statistics.getTimerSecondsLeft() <= 0){
        this.statistics.incrementCorrect();
        this.statistics.decreaseStreak();
        this.resetTime();
      }
      else this.statistics.decreaseTimer();
    }, 1000);
  }

  stopTimer(): void {
    clearInterval(this.timer);
    this.resetTime();
  }

  setTimer(isEnabled: boolean){
    this.isTimerEnabled = isEnabled;
    if(this.isTimerEnabled) {
      this.resetTime();
      this.startTimer();
    }
    else this.stopTimer();
  }

  public resetTime(){
    this.statistics.resetTimer();
  }

  public registerSuccess(){
    this.statistics.incrementAttempt();
    this.statistics.incrementStreak();
    this.statistics.incrementCorrect();
    if(this.isTimerEnabled){
      this.stopTimer();
      this.startTimer();
    }
  }

  public registerFailure(){
    this.wordErrors += this._currentWord.getValue()?.word + ",";
    this.statistics.incrementAttempt();
    this.statistics.resetStreak();
  }

  public getConjugation(){
    return this.isConjugation;
  }

  public setConjugation(isConjugation : boolean){
    this._isLanguageReversed.next(false);
    this.isConjugation = isConjugation;
    if(this.isConjugation){
      this.filteredWords = this.words.filter(w => w.conjugation);
    } else {
      this.filteredWords = this.words.filter(w => w.languageType === WordLanguageType.SpanishRussian);
    }
    this.setAnyWord();
  }

  public validateAnswer(userAnswer: string): boolean{
    if(userAnswer === undefined || userAnswer.trim() === ''){
      this.registerFailure();
      return false;
    }
    let word = this._currentWord;
    let lowerTranslation = userAnswer.toLowerCase();
    // if set
    if(word){
      // classic
      if(!this.isConjugation){
        // word -> one of translations from word.translations
        if(!this._isLanguageReversed.value){
          let filtered = word.value?.translations?.filter(w => lowerTranslation === w.toLowerCase());
          if(filtered && filtered.length > 0){
            this.registerSuccess();
            return true;
          }
          this.registerFailure();
          return false;
        }
        // word.word === translations
        else {
          if(word.value?.word === lowerTranslation){
            this.registerSuccess();
            return true;
          }
          else {
            this.registerFailure();
            return false;
          }
        }
      }
      else return false;
    }
    else return false;
  }

  public setAnyWord(){
    if(this.filteredWords.length == 0){
      this._currentWord.next(null);
      return;
    }
    if(this.isRepeatWords){
      const randomIndex = Math.floor(Math.random() * this.getWordsCount());
      let word = this.filteredWords[randomIndex];
      this._currentWord.next(word);
    }
    else{
      if(this.wordIndex > this.getWordsCount() - 1){
        this.wordIndex = 0;
      }
      let word = this.filteredWords[this.wordIndex];
      this._currentWord.next(word);
      this.wordIndex ++;
    }
  }

  public setLanguageReversed(isReversed: boolean){
    this._isLanguageReversed.next(isReversed);
  }

  public filterWords(filters: string[]| undefined, language: WordLanguageType){
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

  get dataVariable$() {
    return this._currentWord.asObservable();
  }

  get isLanguageReversed$(){
    return this._isLanguageReversed.asObservable();
  }

  public setRepeatWords(isRepeat: boolean){
    this.isRepeatWords = isRepeat;
    this.wordIndex = 0;
  }

  public isTimerSet(): boolean{
    return this.isTimerEnabled;
  }

  public getWordsErrors(){
    return this.wordErrors;
  }

  public getAllowedFilters(): string[] {
    return Array.from(this.allowedFilters);
  }

  public repeatWords(words: string| undefined){
    if(!words) {
      return;
    }
    let errors = new Set(words.split(',').map(attr => attr.trim()));
    this.filteredWords = this.words.filter(item => {
      if (!item.word) {
        return false;
      }
      return errors.has(item.word);
    });
  }

  public getAllowedLanguages(){
    return Array.from(this.allowedLanguages);
  }
}
