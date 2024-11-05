import {Injectable, OnInit} from "@angular/core";
import {WordLanguageType, WordModel} from "../shared/main.api";
import {ApiClient} from "./api.client";
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class GameService{
  private words: WordModel[] = [];
  private filteredWords: WordModel[] = [];
  private allowedFilters: Set<string> = new Set<string>();
  private wordIndex = 0;
  // stats
  private correctAnswersCount = 0;
  private totalAnswers = 0;
  private answersStreak = 0;
  // enable timer if requested
  private isTimerEnabled = false;
  private milliseconds: number = 0;
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

  constructor(private client : ApiClient) {
    this.loadWords();
  }

  async loadWords(){
    this.allowedFilters.clear();
    let apiResult = await this.client.getWords();
    if(apiResult){
      this.words = apiResult.items!!;
      for (let i = 0; i < this.words.length; i++) {
        let attribute = this.words[i].attributes;
        if(attribute){
          attribute.split(',').map(item => item.trim()).forEach(item => this.allowedFilters.add(item));
        }
      }
      this.filteredWords = apiResult.items!!.filter(w => w.languageType === WordLanguageType.SpanishRussian);
      this.setAnyWord();
    }
    else alert('Unable to fetch words from server');
  }

  finish(){
    this.correctAnswersCount = 0;
    this.totalAnswers = 0;
    this.answersStreak = 0;
  }

  getWordsCount() : number{
    return this.filteredWords.length;
  }

  getAttempts(): number{
    return this.totalAnswers;
  }

  getCorrectAnswers(): number{
    return this.correctAnswersCount;
  }

  getStreakCounter(): number{
    return this.answersStreak;
  }

  getTimerSecondsLeft(): number{
    return this.milliseconds / 1000;
  }

  startTimer(): void {
    this.timer = setInterval(() => {
      console.log(this.milliseconds);
      if(this.milliseconds <= 0){
        this.totalAnswers ++;
        this.answersStreak --;
        this.resetTime();
      }
      else this.milliseconds -= 1000;
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
    this.milliseconds = this.defaultTimerMsc;
  }

  public registerSuccess(){
    this.totalAnswers ++;
    this.answersStreak ++;
    this.correctAnswersCount ++;
    if(this.isTimerEnabled){
      this.stopTimer();
      this.startTimer();
    }
  }

  public registerFailure(){
    this.wordErrors += this._currentWord.getValue()?.word + ",";
    this.totalAnswers ++;
    this.answersStreak --;
  }

  public getConjugation(){
    return this.isConjugation;
  }

  public setConjugation(isConjugation : boolean){
    this._isLanguageReversed.next(false);
    this.isConjugation = isConjugation;
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

  public filterWords(filters: string[]| undefined){
    if(filters === undefined || filters.length < 1) {
      this.filteredWords = this.words.filter(w => w.languageType === WordLanguageType.SpanishRussian);
      return;
    }
    this.filteredWords = this.words.filter(item => {
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
}
