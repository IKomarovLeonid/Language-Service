import {Component, OnDestroy} from '@angular/core';
import {WordModel} from "../shared/main.api";
import {GameService} from "../services/game.service";
import {Subscription} from "rxjs";
import {HistoryService} from "../services/history.service";

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

  // for user
  userShowMessage: string | undefined;

    constructor(private gameService : GameService, private service: HistoryService) {
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
    if(result) {
      this.userShowMessage = undefined;
      this.gameService.setAnyWord();
    }
    else{
      this.userShowMessage = `Error. Correct translation of '${word?.word}' -> '${word?.translations}'`;
    }
    this.userTranslation = undefined;
  }

  async finishAttempt(){
      let answers = this.gameService.getAttempts();
      if(answers == 0){
        alert('No attempts has been made yet');
        return;
      }
      let correct = this.gameService.getCorrectAnswers();
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
    return this.gameService.getAttempts();
  }

  showCorrectAnswers(): number{
    return this.gameService.getCorrectAnswers();
  }

  showCurrentStreak(): number{
      return this.gameService.getStreakCounter();
  }

  public isConjugation(){
      return this.gameService.getConjugation();
  }

}
