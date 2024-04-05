import {Injectable} from "@angular/core";
import {AttemptHistoryModel, AttemptModel, LanguageType, WordCategory, WordModel, WordType} from "../shared/main.api";
import {ApiClient} from "./api.client";
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class GameService{
  private words: WordModel[] = [];
  private filteredWords: WordModel[] = [];
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
  private languageType = LanguageType.SpanishRussian;

  // conjugation
  private isConjugation = false;
  private _isLanguageReversed = new BehaviorSubject<boolean>(false);

  // answers -> for history
  answers: AttemptModel[] = [];

  constructor(private client : ApiClient) {
    this.loadWords();
  }

  async loadWords(){
    let apiResult = await this.client.getWords();
    if(apiResult){
      this.words = apiResult.items!!;
      this.filteredWords = apiResult.items!!.filter(w => w.language === this.languageType);
      this.setAnyWord();
    }
    else alert('Unable to fetch words from server');
  }

  finish(){
    this.answers = [];
    this.correctAnswersCount = 0;
    this.totalAnswers = 0;
    this.answersStreak = 0;
  }

  getUserAnswers(){
    return this.answers;
  }

  getWordsCount() : number{
    return this.filteredWords.length;
  }

  getTotalAttempts(): number{
    return this.totalAnswers;
  }

  getCorrectAnswers(): number{
    return this.correctAnswersCount;
  }

  private saveAttempt(word: string, expectedTranslation: string[], userAnswer: string, isCorrect : boolean){
    let model = new AttemptModel();
    model.userTranslation = userAnswer;
    model.word = word;
    model.expectedTranslations = expectedTranslation;
    model.isCorrect = isCorrect;
    // model.totalSeconds = this.gameService.getInitialSeconds() - this.gameService.getTimerSecondsLeft();
    model.totalSeconds = 0;
    this.answers.push(model);
  }

  getStreakCounter(): number{
    return this.answersStreak;
  }

  getTimerSecondsLeft(): number{
    return this.milliseconds / 1000;
  }

  getInitialSeconds(): number{
    return this.defaultTimerMsc / 1000;
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
    this.totalAnswers ++;
    this.answersStreak --;
  }

  public getConjugation(){
    return this.isConjugation;
  }

  public setConjugation(isConjugation : boolean){
    this._isLanguageReversed.next(false);
    this.isConjugation = isConjugation;
    this.filterWords(WordCategory.Any, WordType.Any);
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
            this.saveAttempt(word.value?.word!!, word.value?.translations!!, lowerTranslation, true);
            return true;
          }
          this.registerFailure();
          this.saveAttempt(word.value?.word!!, word.value?.translations!!, lowerTranslation, false);
          return false;
        }
        // word.word === translations
        else {
          if(word.value?.word === lowerTranslation){
            this.registerSuccess();
            let expected: string[] = [word.value?.word];
            this.saveAttempt(word.value?.translations!! [0], expected, lowerTranslation, true);
            return true;
          }
          else {
            this.registerFailure();
            let expected: string[] = [word.value?.word!!];
            this.saveAttempt(word.value?.translations!! [0]!!, expected, lowerTranslation, false);
            return false;
          }
        }
      }
      else return false;
    }
    else return false;
  }

  public setAnyWord(){
    const randomIndex = Math.floor(Math.random() * this.getWordsCount());
    let word = this.filteredWords[randomIndex];
    this._currentWord.next(word);
  }

  public setLanguageReversed(isReversed: boolean){
    this._isLanguageReversed.next(isReversed);
  }

  public filterWords(category: WordCategory, type: WordType){
    if(this.words){
      if(this.isConjugation){
        this.filteredWords = this.words.filter(w => w.conjugation);
      }
      else{
        let byLanguage = this.words.filter(w => w.language === this.languageType);
        if(category === WordCategory.Any && type === WordType.Any){
          this.filteredWords = byLanguage;
          return;
        }
        if(category === WordCategory.Any){
          this.filteredWords = byLanguage.filter( item => item.type === type);
        }
        else{
          this.filteredWords = byLanguage.filter( item => item.category === category);
        }
      }
    }
  }

  get dataVariable$() {
    return this._currentWord.asObservable();
  }

  get isLanguageReversed$(){
    return this._isLanguageReversed.asObservable();
  }

  public setRetryWords(historyAttempt: AttemptHistoryModel){
    let errors = historyAttempt.errors;
    if(errors) {
      let len = Object.keys(errors).length;
      if (len <= 0) {
        alert('No words to repeat from this history');
        return;
      }
      let words = new Array<WordModel>();
      for (const p in errors) {
        if (errors.hasOwnProperty(p)) {
          if (this.words) {
            let word = this.words.filter(w => w.word === p);
            if (word.length > 0) words.push(word[0]);
            else {
              let filtered = this.words.filter(w =>
                w.translations!!.filter(w =>
                  w === p).length > 0);
              if (filtered.length > 0) words.push(filtered[0]);
            }
          }
          this.finish();
          this.filteredWords = words;
        }
      }
    }
  }

}
