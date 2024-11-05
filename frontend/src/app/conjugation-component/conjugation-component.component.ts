import {Component, OnDestroy, OnInit} from '@angular/core';
import {GameService} from "../../services/game.service";
import {WordModel} from "../../shared/main.api";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-conjugation-component',
  templateUrl: './conjugation-component.component.html',
  styleUrls: ['./conjugation-component.component.css']
})
export class ConjugationComponentComponent implements OnDestroy{

  yo: string | undefined;
  tu: string | undefined;
  el: string | undefined;
  nosotros: string | undefined;
  vosotros: string | undefined;
  ustedes: string | undefined;

  message: string | undefined;
  correctAnswers: string | undefined;

  word: WordModel | null | undefined;
  private dataSubscription: Subscription;

  wrongItems: boolean[];
  constructor(private service: GameService) {
    this.wrongItems = [false, false, false, false, false, false];
    this.dataSubscription = this.service.dataVariable$.subscribe(value => {
      this.word = value;
    });
  }

  makeAnswer(){
    this.message = undefined;
    this.correctAnswers = undefined;
    if (this.yo === undefined || this.tu === undefined || this.el === undefined ||
      this.nosotros === undefined || this.vosotros === undefined || this.ustedes === undefined) {
      this.message = "Error: At least one input field is not filled";
    }
    if(this.word?.conjugation != undefined){
      let expected = this.word?.conjugation?.split(",");
      if(expected?.length != 6) this.message = 'No conjugations for word: ' + this.word?.word + ' got: ' + this.word?.conjugation;
      let correct = 0;
      if(this.yo === expected[0]) {
        this.setWrongFlag(0, false);
        correct ++;
      }
      else this.setWrongFlag(0, true);
      if(this.tu === expected[1]){
        this.setWrongFlag(1, false);
        correct ++;
      }
      else this.setWrongFlag(1, true);
      if(this.el === expected[2]) {
        this.setWrongFlag(2, false);
        correct ++;
      }
      else this.setWrongFlag(2, true);
      if(this.nosotros === expected[3]) {
        this.setWrongFlag(3, false);
        correct ++;
      }
      else this.setWrongFlag(3, true);
      if(this.vosotros === expected[4]) {
        this.setWrongFlag(4, false);
        correct ++;
      }
      else this.setWrongFlag(4, true);
      if(this.ustedes === expected[5]) {
        this.setWrongFlag(5, false);
        correct ++;
      }
      else this.setWrongFlag(5, true);
      this.message = `Correct answers: ${correct}`;
      if(correct === 6) {
        this.message = undefined;
        this.service.registerSuccess();
        this.service.setAnyWord();
        this.yo = this.tu = this.nosotros = this.el = this.vosotros = this.ustedes = undefined;
      }
      else{
        this.service.registerFailure();
        this.correctAnswers = expected.toString();
      }
    }
  }

  private setWrongFlag(index: number, flag: boolean){
    this.wrongItems[index] = flag;
  }

  ngOnDestroy() {
    this.dataSubscription.unsubscribe();
  }
}
