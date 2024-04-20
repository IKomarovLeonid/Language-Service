import {Component, Input} from '@angular/core';
import {LanguageType, WordCategory, WordType} from "../../shared/main.api";
import {GameService} from "../../services/game.service";

@Component({
  selector: 'app-filter-component',
  templateUrl: './filter-component.component.html',
  styleUrls: ['./filter-component.component.css']
})
export class FilterComponentComponent {

  // filtration
  @Input() selectedEnumCategory = WordCategory.Any;
  @Input() selectedEnumType = WordType.Any;
  @Input() selectedEnumLanguage: LanguageType = LanguageType.SpanishRussian;
  @Input() enumCategoryValues: WordCategory[] = Object.values(WordCategory);
  @Input() enumTypeValues: WordType[] = Object.values(WordType);
  @Input() enumLanguageValues: LanguageType[] = Object.values(LanguageType);


  languageTypeMapping = {
    [LanguageType.SpanishRussian]: 'Spanish <-> Russian',
    [LanguageType.EnglishRussian]: 'English <-> Russian',
  };

  constructor(private gameService: GameService) {
  }

  filterWords(){
    this.gameService.filterWords(this.selectedEnumCategory, this.selectedEnumType, this.selectedEnumLanguage);
    if(this.gameService.getWordsCount() === 0) {
      alert(`No words of this filters '${this.selectedEnumType}' and type '${this.selectedEnumCategory}'`);
      this.selectedEnumType = WordType.Any;
      this.selectedEnumCategory = WordCategory.Any;
      this.gameService.filterWords(this.selectedEnumCategory, this.selectedEnumType, this.selectedEnumLanguage);
    }
    this.gameService.setAnyWord();
  }

  public isConjugation(){
    return this.gameService.getConjugation();
  }
}
