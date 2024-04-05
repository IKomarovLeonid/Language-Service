import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {AttemptHistoryModel, AttemptModel, LanguageType, WordCategory, WordModel, WordType} from "../shared/main.api";
import {ApiClient} from "../services/api.client";
import {GameService} from "../services/game.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy{
  userTranslation: string | undefined;
  // filtration
  @Input() selectedEnumCategory = WordCategory.Any;
  @Input() selectedEnumType = WordType.Any;
  @Input() enumLanguage: LanguageType = LanguageType.SpanishRussian;
  @Input() enumCategoryValues: WordCategory[] = Object.values(WordCategory);
  @Input() enumTypeValues: WordType[] = Object.values(WordType);
  @Input() enumLanguageValues: LanguageType[] = Object.values(LanguageType);

  // for attempt history
  history: AttemptHistoryModel[] | undefined;
  // toggles
  isRepeatWords = true;
  isTimerEnabled = false;
  isConjugation = false;
  isLanguageReversed = false;
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

  ngOnInit(): void {
      this.loadHistory();
  }

  ngOnDestroy() {
    this.dataSubscription.unsubscribe();
  }

  async loadHistory(){
    let apiResult = await this.client.getHistory();
    if(apiResult){
      this.history = apiResult.items!!;
    }
    else alert('Unable to fetch history from server');
  }

  makeAnswer(){
    let result = this.gameService.validateAnswer(this.userTranslation!!);
    let word = this.word;
    if(result) {
      this.userShowMessage = undefined;
      this.gameService.setAnyWord();
    }
    else{
      this.userShowMessage = !this.isLanguageReversed ?
        `Error. Correct translation of '${word?.word}' -> '${word?.translations}'`:
        `Error. Correct translation of '${word?.translations!![0]}' -> '${word?.word}'`
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
    this.userShowMessage = undefined;
    this.gameService.setAnyWord();
    this.gameService.resetTime();
    this.loadHistory();
  }

  showHistoryInfo(attempt: AttemptHistoryModel){
    let combinedString = Object.entries(attempt.errors!!)
      .map(([key, value]) => `'${key}' errors was '${value}' times`)
      .join('\n');
    alert(combinedString);
  }

  onReverseLanguage(){
    if(!this.isLanguageReversed){
      this.gameService.setLanguageReversed(true);
    }
    else{
      this.gameService.setLanguageReversed(false);
    }
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

  onEnableTimer(){
    this.gameService.setTimer(!this.isTimerEnabled);
  }

  showTimerSecondsLeft(): number{
    return this.gameService.getTimerSecondsLeft();
  }

  async onDeleteHistory(id: number | undefined){
      let result = await this.client.deleteHistory(id!!);
      if(!result) alert('Failed to delete history attempt');
      this.loadHistory();
  }

  onRepeatHistory(id: number | undefined){
      if(id){
        let model = this.history?.filter(h => h.id === id)[0]!!;
        this.gameService.setRetryWords(model);
        this.gameService.setAnyWord();
      }
      else alert('This history has undefined id')
  }

  onConjugation(){
    if(!this.isConjugation){
      this.gameService.setConjugation(true);
    }
    else{
      this.gameService.setConjugation(false);
    }
    this.filterWords();
    this.gameService.setAnyWord();
  }

  filterWords(){
      this.gameService.filterWords(this.selectedEnumCategory, this.selectedEnumType);
      this.gameService.setAnyWord();
  }
}
