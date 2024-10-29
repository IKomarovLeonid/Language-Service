import {Component, Input} from '@angular/core';
import {WordLanguageType} from "../../shared/main.api";
import {GameService} from "../../services/game.service";

@Component({
  selector: 'app-filter-component',
  templateUrl: './filter-component.component.html',
  styleUrls: ['./filter-component.component.css']
})
export class FilterComponentComponent {

  // filtration
  @Input() selectedEnumLanguage: WordLanguageType = WordLanguageType.SpanishRussian;

  constructor(private gameService: GameService) {
  }

  filterWords(){
    this.gameService.setAnyWord();
  }

  public isConjugation(){
    return this.gameService.getConjugation();
  }
}
