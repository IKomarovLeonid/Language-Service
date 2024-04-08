import {Component, Input, OnDestroy} from '@angular/core';
import {LanguageType, WordCategory, WordModel, WordType} from "../shared/main.api";
import {ApiClient} from "../services/api.client";
import {GameService} from "../services/game.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnDestroy{
  userTranslation: string | undefined;
  // filtration
  @Input() selectedEnumCategory = WordCategory.Any;
  @Input() selectedEnumType = WordType.Any;
  @Input() selectedEnumLanguage: LanguageType = LanguageType.SpanishRussian;
  @Input() enumCategoryValues: WordCategory[] = Object.values(WordCategory);
  @Input() enumTypeValues: WordType[] = Object.values(WordType);
  @Input() enumLanguageValues: LanguageType[] = Object.values(LanguageType);

  // message
  userShowMessage: string | undefined;
  word: WordModel | null | undefined;
  private dataSubscription: Subscription;

  languageTypeMapping = {
    [LanguageType.SpanishRussian]: 'Spanish <-> Russian',
    [LanguageType.EnglishRussian]: 'English <-> Russian',
  };

    constructor(private client : ApiClient, private gameService : GameService) {
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
      this.resetMessage();
      this.gameService.setAnyWord();
    }
    else{
      this.userShowMessage = `Error. Correct translation of '${word?.word}' -> '${word?.translations}'`;
    }
    this.userTranslation = undefined;
  }

  async finishAttempt(){
      let attempts = this.gameService.getUserAnswers();
    await this.client.createAttempt(
      attempts,
      this.gameService.getCorrectAnswers(), 30,
      this.selectedEnumType,
      this.selectedEnumCategory);
    this.gameService.finish();
    this.userTranslation = undefined;
    this.resetMessage();
    this.gameService.setAnyWord();
    this.gameService.resetTime();
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

  showTimerSecondsLeft(): number{
    return this.gameService.getTimerSecondsLeft();
  }

  filterWords(){
      this.resetMessage();
      this.gameService.filterWords(this.selectedEnumCategory, this.selectedEnumType, this.selectedEnumLanguage);
      this.gameService.setAnyWord();
      if(this.gameService.getWordsCount() === 0) this.userShowMessage = 'No words found';
  }

  public isConjugation(){
      return this.gameService.getConjugation();
  }

  private resetMessage(){
      this.userShowMessage = undefined;
  }

}
