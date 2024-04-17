import {Component, OnDestroy} from '@angular/core';
import {GameService} from "../../services/game.service";
import {WordModel} from "../../shared/main.api";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-word-info-component',
  templateUrl: './word-info-component.component.html',
  styleUrls: ['./word-info-component.component.css']
})
export class WordInfoComponentComponent implements OnDestroy{

  word: WordModel | null | undefined;
  private dataSubscription: Subscription;
  private isReversedSubscription: Subscription;
  isReversed: boolean | undefined;
  constructor(private gameService: GameService) {
    this.dataSubscription = this.gameService.dataVariable$.subscribe(value => {
      this.word = value;
    });
    this.isReversedSubscription = this.gameService.isLanguageReversed$.subscribe(value => {
      this.isReversed = value;
    });
  }

  public isConjugation(){
    return this.gameService.getConjugation();
  }

  public isTimerSet(){
    return this.gameService.isTimerSet();
  }

  public hasWords(){
    return this.gameService.getWordsCount() > 0;
  }

  ngOnDestroy() {
    this.dataSubscription.unsubscribe();
    this.isReversedSubscription.unsubscribe();
  }


  showTimerSecondsLeft(): number{
    return this.gameService.getTimerSecondsLeft();
  }
}
