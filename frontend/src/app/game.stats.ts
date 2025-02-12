import {Injectable} from "@angular/core";

@Injectable({"providedIn": 'root'})
export class GameStats{
  // stats
  private correctAnswersCount = 0;
  private totalAnswers = 0;
  private  answersStreak = 0;
  private millisecondsLeft: number = 0;
  constructor() {
  }

  public reset(){
    this.correctAnswersCount = 0;
    this.millisecondsLeft = 0;
    this.answersStreak = 0;
    this.totalAnswers = 0;
    this.millisecondsLeft = 10000;
  }

  incrementAttempt(){
    this.totalAnswers ++;
  }

  incrementCorrect(){
    this.correctAnswersCount ++;
  }

  resetStreak(){
    this.answersStreak = 0;
  }

  incrementStreak(){
    this.answersStreak++;
  }

  decreaseStreak(){
    this.answersStreak--;
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
    return this.millisecondsLeft / 1000;
  }

  resetTimer(){
    this.millisecondsLeft = 10000;
  }

  decreaseTimer(){
    this.millisecondsLeft -= 1000;
  }
}
