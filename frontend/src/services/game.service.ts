import {Injectable} from "@angular/core";
import {LanguageType, WordCategory, WordModel, WordType} from "../shared/main.api";
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
  private isLanguageReversed = false;

  constructor(private client : ApiClient) {
    console.log('loading ctor...');
    this.loadWords();
    console.log('loading ctor... done');
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

  setWords(words: WordModel[]): void {
    this.words = words;
    this.wordIndex = 0;
  }

  finish(){
    this.correctAnswersCount = 0;
    this.totalAnswers = 0;
    this.answersStreak = 0;
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

  getStreakCounter(): number{
    return this.answersStreak;
  }

  getRandomWord(isRepeatWords: boolean) {
    if(isRepeatWords){
      const randomIndex = Math.floor(Math.random() * this.getWordsCount());
      let word = this.words[randomIndex];
      this.updateDataVariable(word);
    }
    else{
      let len = this.getWordsCount();
      if(this.wordIndex < len){
        let word = this.words[this.wordIndex];
        this.updateDataVariable(word);
        this.wordIndex ++;
      }
      else{
        this.wordIndex = 0;
        let word = this.words[this.wordIndex];
        this.updateDataVariable(word);
      }
    }
  }

  checkAnswer(translation: string | undefined, expectedTranslations: string[], isConjugation: boolean): boolean{
    this.totalAnswers ++;
    if(translation === undefined || translation.trim() === ''){
      this.answersStreak--;
      return false;
    }
    let lowerTranslation = translation.toLowerCase();
    if(!isConjugation){
      let filtered = expectedTranslations.filter(w => lowerTranslation === w.toLowerCase());
      if(filtered.length > 0){
        this.registerSuccess();
        return true;
      }
    }
    else {
      let answers = translation.split(",");
      if(answers.length != expectedTranslations.length){
        this.answersStreak--;
        return false;
      }
      for(let i = 0; i < answers.length; i++){
        if(answers[i] != expectedTranslations[i]){
          this.answersStreak--;
          return false;
        }
      }
      this.registerSuccess();
      return true;
    }

    this.answersStreak--;
    return false;
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
    this.answersStreak ++;
    this.correctAnswersCount ++;
    if(this.isTimerEnabled){
      this.stopTimer();
      this.startTimer();
    }
  }

  public getConjugation(){
    return this.isConjugation;
  }

  public setConjugation(isConjugation : boolean){
    this.isConjugation = isConjugation;
    this.filterWords(WordCategory.Any, WordType.Any);
    this.setAnyWord();
  }

  get dataVariable$() {
    return this._currentWord.asObservable();
  }

  updateDataVariable(value: WordModel) {
    this._currentWord.next(value);
  }

  public validateAnswer(userAnswer: string): boolean{
    this.totalAnswers ++;
    if(userAnswer === undefined || userAnswer.trim() === ''){
      this.answersStreak--;
      return false;
    }
    let word = this._currentWord;
    let lowerTranslation = userAnswer.toLowerCase();
    // if set
    if(word){
      // classic
      if(!this.isConjugation){
        // word -> one of translations from word.translations
        if(!this.isLanguageReversed){
          let filtered = word.value?.translations?.filter(w => lowerTranslation === w.toLowerCase());
          if(filtered && filtered.length > 0){
            this.registerSuccess();
            return true;
          }
          this.answersStreak--;
          return false;
        }
        // word.word === translations
        else {
          if(word.value?.word === lowerTranslation){
            this.registerSuccess();
            return true;
          }
          else {
            this.answersStreak--;
            return false;
          }
        }
      }
      // verbs -> conjugation
      else{
        return true;
      }
    }
    else return false;
  }

  public setAnyWord(){
    const randomIndex = Math.floor(Math.random() * this.getWordsCount());
    let word = this.filteredWords[randomIndex];
    this.updateDataVariable(word);
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

}
