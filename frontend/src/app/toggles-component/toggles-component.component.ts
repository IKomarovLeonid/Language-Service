import {Component} from '@angular/core';
import {GameService} from "../../services/game.service";
import {WordLanguageType} from "../../shared/main.api";

@Component({
  selector: 'app-toggles-component',
  templateUrl: './toggles-component.component.html',
  styleUrls: ['./toggles-component.component.css']
})
export class TogglesComponentComponent {

  // toggles
  isRepeatWords = true;
  isTimerEnabled = false;
  isConjugation = false;
  isLanguageReversed = false;
  isRandom = false;

  constructor(private gameService: GameService) {
  }

  onReverseLanguage(){
    if(!this.isLanguageReversed){
      this.gameService.setLanguageReversed(true);
    }
    else{
      this.gameService.setLanguageReversed(false);
    }
    this.gameService.setAnyWord();
  }

  onConjugation(){
    if(!this.isConjugation){
      this.gameService.stopTimer();
      this.gameService.setTimer(false);
      this.gameService.setConjugation(true);
    }
    else{
      this.gameService.setConjugation(false);
    }
    this.gameService.setAnyWord();
  }

  onRepeatWords(){
    if(!this.isRepeatWords){
      this.gameService.setRepeatWords(true);
    }
    else{
      this.gameService.setRepeatWords(false);
    }
    this.gameService.setAnyWord();
  }

  onEnableTimer(){
    if(!this.isTimerEnabled){
      this.gameService.setTimer(true);
    }
    else{
      this.gameService.setTimer(false);
    }
  }

  onRandomGame(){
    alert('implement')
  }
}
