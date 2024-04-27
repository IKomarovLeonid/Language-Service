import {Component, OnDestroy} from '@angular/core';
import {WordModel} from "../shared/main.api";
import {ApiClient} from "../services/api.client";
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
      let attempts = this.gameService.getUserAnswers();
      if(attempts.length < 1) alert('No attempts. Please make at least one answer');
      else{
        await this.service.createHistory(
          attempts,
          this.gameService.getCurrentWordType(),
          this.gameService.getCurrentWordCategory(),
          this.gameService.getCorrectAnswers());
        this.gameService.finish();
        this.userTranslation = undefined;
        this.userShowMessage = undefined;
        this.gameService.setAnyWord();
        this.gameService.resetTime();
        await this.service.loadHistory();
      }
  }

  showTotalCount(): number{
      return this.gameService.getWordsCount();
  }

  showTotalAnswers(): number{
    return this.gameService.getTotalAttempts();
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
