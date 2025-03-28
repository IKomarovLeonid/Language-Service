import {Component, OnDestroy} from '@angular/core';
import {WordModel} from "../shared/main.api";
import {GameService} from "../services/game.service";
import {Subscription} from "rxjs";
import {HistoryService} from "../services/history.service";
import {GameStats} from "./game.stats";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnDestroy{
  // word
  userTranslation: string | undefined;
  word: WordModel | null | undefined;
  private dataSubscription: Subscription;
  private accuracies: { [key: string]: { correct: number, wrong: number } } = {
  };

  // for user
  userShowMessage: string | undefined;

    constructor(private gameService : GameService, private service: HistoryService, private statistics: GameStats) {
      this.dataSubscription = this.gameService.dataVariable$.subscribe(value => {
        this.word = value;
      });
  }

  ngOnDestroy() {
    this.dataSubscription.unsubscribe();
  }

  makeAnswer(){
    let result = this.gameService.validateAnswer(this.userTranslation!!);
    let word = this.word;
    this.registerAccuracy(result, word?.word!!);
    if(result) {
      this.userShowMessage = undefined;
      this.gameService.setAnyWord();
    }
    else{
      this.userShowMessage = `Correct translation of '${word?.word}' -> '${word?.translations}'`;
    }
    this.userTranslation = undefined;
  }

  async finishAttempt(){
      let answers = this.statistics.getAttempts();
      if(answers == 0){
        alert('No attempts has been made yet');
        return;
      }
      let correct = this.statistics.getCorrectAnswers();
    await this.service.createHistory(
      answers,
      correct,
      this.gameService.getWordsErrors()
    );
    this.gameService.finish();
    this.userTranslation = undefined;
    this.userShowMessage = undefined;
    this.gameService.setAnyWord();
    this.gameService.resetTime();
    this.service.loadHistory();
  }

  showTotalCount(): number{
      return this.gameService.getWordsCount();
  }

  showTotalAnswers(): number{
    return this.statistics.getAttempts();
  }

  showCorrectAnswers(): number{
    return this.statistics.getCorrectAnswers();
  }

  showCurrentStreak(): number{
      return this.statistics.getStreakCounter();
  }

  public isConjugation(){
      return this.gameService.getConjugation();
  }

  private registerAccuracy(isSuccess: boolean, word: string){
      if(!this.accuracies[word]){
        this.accuracies[word] = {correct: isSuccess ? 1 : 0, wrong: isSuccess ? 0 : 1};
      } else {
        if(isSuccess) this.accuracies[word].correct ++;
        else this.accuracies[word].wrong++;
      }
  }

  public getAccuracy(){
      if(this.word){
        let word = this.word.word!!;
        if(this.accuracies[word]){
          let accuracy = this.accuracies[word];
          return accuracy.correct / (accuracy.wrong + accuracy.correct) * 100;
        }
      }
      return 0;
  }
}
