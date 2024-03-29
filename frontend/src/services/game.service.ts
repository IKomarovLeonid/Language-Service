import {Injectable} from "@angular/core";
import {WordModel} from "../shared/main.api";

@Injectable({
  providedIn: 'root'
})
export class GameService{
  private words: WordModel[] = [];
  private wordIndex = 0;
  // stats
  private correctAnswersCount = 0;
  private totalAnswers = 0;
  private answersStreak = 0;
  // enable timer if requested
  private isTimerEnabled = false;
  private milliseconds: number = 0;
  private timer: any;
  private defaultTimerMsc;

  constructor() {
    this.defaultTimerMsc = 10000;
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
    return this.words.length;
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

  getRandomWord(isRepeatWords: boolean): WordModel{
    if(isRepeatWords){
      const randomIndex = Math.floor(Math.random() * this.getWordsCount());
      return this.words[randomIndex];
    }
    else{
      let len = this.getWordsCount();
      if(this.wordIndex < len){
        let word = this.words[this.wordIndex];
        this.wordIndex ++;
        return word;
      }
      else{
        this.wordIndex = 0;
        return this.words[this.wordIndex];
      }
    }
  }

  checkAnswer(translation: string | undefined, expectedTranslations: string[]): boolean{
    this.totalAnswers ++;
    if(translation === undefined || translation.trim() === ''){
      this.answersStreak--;
      return false;
    }
    let lowerTranslation = translation.toLowerCase();
    let filtered = expectedTranslations.filter(w => lowerTranslation === w.toLowerCase());
    if(filtered.length > 0){
      this.answersStreak ++;
      this.correctAnswersCount ++;
      if(this.isTimerEnabled){
        this.stopTimer();
        this.startTimer();
      }
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

  public getAnyOfConjugation(word: WordModel) : { translate: string, expected: string } | undefined{
    if(!word.conjugation) return undefined;
    let items = word.conjugation.split(",");
    if(items.length != 6) return undefined;
    let index = Math.floor(Math.random() * items.length);
    let con = items[index];
    let part = '';
    switch (index){
      case 0:
        part = 'yo'
        break;
      case 1:
        part = 'tú'
        break;
      case 2:
        part = 'él/ella'
        break;
      case 3:
        part = 'nosotros'
        break;
      case 4:
        part = 'vosotros'
        break;
      case 5:
        part = 'ellos'
        break;
    }
    let toTranslate = part + " -> " + word.word;
    return {translate: toTranslate, expected: con};
  }
}
