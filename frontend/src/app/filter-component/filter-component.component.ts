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
  selectedTags: Set<string> = new Set<string>();

  constructor(private gameService: GameService) {
  }

  filterWords(){
    this.gameService.filterWords(this.getSelectedFilters())
    this.gameService.setAnyWord();
  }

  public isConjugation(){
    return this.gameService.getConjugation();
  }

  showAllowedFilters(){
    return this.gameService.getAllowedFilters();
  }

  toggleTagSelection(tag: string): void {
    if (this.selectedTags.has(tag)) {
      this.selectedTags.delete(tag);  // Deselect tag
    } else {
      this.selectedTags.add(tag);  // Select tag
    }
    this.gameService.filterWords(this.getSelectedFilters());
    this.gameService.setAnyWord();
  }

  private getSelectedFilters(): string[]{
    return Array.from(this.selectedTags);
  }
}
