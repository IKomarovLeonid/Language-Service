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
  constructor(private gameService: GameService) {
    this.dataSubscription = this.gameService.dataVariable$.subscribe(value => {
      this.word = value;
    });
  }

  public isConjugation(){
    return this.gameService.getConjugation();
  }

  ngOnDestroy() {
    this.dataSubscription.unsubscribe();
  }
}
