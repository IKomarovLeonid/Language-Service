import { Component } from '@angular/core';
import {GameService} from "../../services/game.service";

@Component({
  selector: 'app-conjugation-component',
  templateUrl: './conjugation-component.component.html',
  styleUrls: ['./conjugation-component.component.css']
})
export class ConjugationComponentComponent {

  yo: string | undefined;
  tu: string | undefined;
  el: string | undefined;
  nosotros: string | undefined;
  vosotros: string | undefined;
  ustedes: string | undefined;

  message: string | undefined;

  wrongItems: boolean[];
  constructor(private service: GameService) {
    this.wrongItems = [false, false, false, false, false, false];
  }

  makeAnswer(){
    let word = this.service.getCurrentWord();
    console.log(this.yo, this.tu, this.nosotros, this.vosotros, this.ustedes, this.el);
    this.message = undefined;
    if (this.yo === undefined || this.tu === undefined || this.el === undefined ||
      this.nosotros === undefined || this.vosotros === undefined || this.ustedes === undefined) {
      this.message = "Error: At least one input field is not filled";
    }
    console.log(word);
    console.log(word?.conjugation);
    console.log(word?.conjugation != undefined);
    if(word?.conjugation != undefined){
      let expected = word?.conjugation?.split(",");
      if(expected?.length != 6) this.message = 'Not expected conjugation for word: ' + word?.word + ' got: ' + word?.conjugation;
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
    }
  }

  private setWrongFlag(index: number, flag: boolean){
    this.wrongItems[index] = flag;
  }
}
