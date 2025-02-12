import {Component, OnDestroy} from '@angular/core';
import {GameService} from "../../services/game.service";
import {WordModel} from "../../shared/main.api";
import {Subscription} from "rxjs";
import {GameStats} from "../game.stats";

@Component({
  selector: 'app-word-info-component',
  templateUrl: './word-info-component.component.html',
  styleUrls: ['./word-info-component.component.css']
})
export class WordInfoComponentComponent implements OnDestroy{

  itemsArray: string[] = [];

  word: WordModel | null | undefined;
  private dataSubscription: Subscription;
  private isReversedSubscription: Subscription;
  isReversed: boolean | undefined;
  constructor(private gameService: GameService, private statistics: GameStats) {
    this.dataSubscription = this.gameService.dataVariable$.subscribe(value => {
      this.word = value;
      if (this.word?.attributes) {
        this.itemsArray = this.word?.attributes.split(',').map(item => item.trim());
      } else {
        this.itemsArray = [];
      }
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

  ngOnDestroy() {
    this.dataSubscription.unsubscribe();
    this.isReversedSubscription.unsubscribe();
    this.itemsArray = [];
  }


  showTimerSecondsLeft(): number{
    return this.statistics.getTimerSecondsLeft();
  }
}
