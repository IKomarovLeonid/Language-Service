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
  }

  incrementAttempt(){
    this.totalAnswers ++;
  }

  incrementCorrect(){
    this.correctAnswersCount ++;
  }

  incrementStreak(){
    this.answersStreak++;
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
}
