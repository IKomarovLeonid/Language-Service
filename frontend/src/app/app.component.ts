import {Component, OnInit} from '@angular/core';
import {AttemptHistoryModel, AttemptModel, WordCategory, WordModel, WordType} from "../shared/main.api";
import {ApiClient} from "../services/api.client";
import {GameService} from "../services/game.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  wordsFromServer: WordModel[] | undefined;
  wordToTranslate: string | undefined;
  userTranslation: string | undefined;
  expectedTranslations: string[] | undefined;
  // filtration
  selectedEnumCategory: WordCategory = WordCategory.Any;
  selectedEnumType: WordType = WordType.Any;
  // for attempt history
  history: AttemptHistoryModel[] | undefined;
  answers: AttemptModel[] = [];
  // toggles
  isRepeatWords = true;
  isLanguageReversed = false;
  isTimerEnabled = false;
  // message
  userShowMessage: string | undefined;

    constructor(private client : ApiClient, private gameService : GameService) {
  }

  ngOnInit(): void {
      this.loadWords();
      this.loadHistory();
  }

  async loadWords(){
      let apiResult = await this.client.getWords();
      if(apiResult){
        this.wordsFromServer = apiResult.items!!;
        this.gameService.setWords(apiResult.items!!);
        this.setWord();
      }
      else alert('Unable to fetch words from server');
  }

  async loadHistory(){
    let apiResult = await this.client.getHistory();
    if(apiResult){
      this.history = apiResult.items!!;
    }
    else alert('Unable to fetch history from server');
  }

  setWord() : void{
    let word = this.gameService.getRandomWord(this.isRepeatWords);
    if(word){
      this.resetErrorMessage();
      if(!this.isLanguageReversed){
        this.wordToTranslate = word.word;
        this.expectedTranslations = word.translations!!;
      }
      else {
        this.wordToTranslate = word.translations!![0];
        this.expectedTranslations = [];
        this.expectedTranslations.push(word.word!!);
      }
    }
    else {
      this.wordToTranslate = undefined;
      this.userTranslation = undefined;
      this.expectedTranslations = undefined;
      this.userShowMessage = 'No words by this category and language type';
    }
  }


  makeAnswer(){
      let result = this.gameService.checkAnswer(this.userTranslation, this.expectedTranslations!!)
      this.saveAttempt(this.wordToTranslate!!, this.expectedTranslations!!,
      this.userTranslation ?? 'N/A', result);
      this.userTranslation = undefined;
      if(!result){
        this.buildErrorMessage();
      }
      else {
        this.resetErrorMessage();
        this.setWord();
      }
  }

  filterWords(){
      if(this.wordsFromServer){
        if(this.selectedEnumCategory === WordCategory.Any && this.selectedEnumType === WordType.Any){
          this.gameService.setWords(this.wordsFromServer);
        }
        else {
          if(this.selectedEnumCategory === WordCategory.Any){
            this.gameService.setWords(this.wordsFromServer.filter(item => item.type === this.selectedEnumType));
          }
          else{
            if(this.selectedEnumType === WordType.Any){
              this.gameService.setWords(this.wordsFromServer.filter(item => item.category === this.selectedEnumCategory));
            }
            else this.gameService.setWords(this.wordsFromServer.filter(item =>
              item.category === this.selectedEnumCategory &&
              item.type === this.selectedEnumType
            ));
          }
        }
      }
      this.setWord();
  }

  async finishAttempt(){
    await this.client.createAttempt(
      this.answers,
      this.gameService.getCorrectAnswers(), 30,
      this.selectedEnumType,
      this.selectedEnumCategory);
    this.gameService.finish();
    this.userTranslation = undefined;
    this.answers = [];
    this.resetErrorMessage();
    this.setWord();
    this.loadHistory();
  }

  showHistoryInfo(attempt: AttemptHistoryModel){
    let combinedString = Object.entries(attempt.errors!!)
      .map(([key, value]) => `'${key}' errors was '${value}' times`)
      .join('\n');
    alert(combinedString);
  }

  saveAttempt(word: string, expectedTranslation: string[], userAnswer: string, isCorrect : boolean){
      let model = new AttemptModel();
      model.userTranslation = userAnswer;
      model.word = word;
      model.expectedTranslations = expectedTranslation;
      model.isCorrect = isCorrect;
      model.totalSeconds = 10;
      this.answers.push(model);
  }

  buildErrorMessage(){
      this.userShowMessage = `Error. Correct translation of ${this.wordToTranslate} -> ${this.expectedTranslations}`;
  }

  resetErrorMessage(){
      this.userShowMessage = undefined;
  }

  onReverseLanguage(){
      if(this.expectedTranslations && this.wordToTranslate){
        if(!this.isLanguageReversed){
          let word = this.wordToTranslate;
          this.wordToTranslate = this.expectedTranslations[0];
          this.expectedTranslations = [];
          this.expectedTranslations.push(word);
        }
        else{
          let word = this.expectedTranslations[0];
          this.expectedTranslations = [];
          this.expectedTranslations.push(this.wordToTranslate);
          this.wordToTranslate = word;
        }
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

  protected readonly Category = Object;
  protected readonly WordCategory = WordCategory;

  protected readonly Type = Object;
  protected readonly WordType = WordType;

  onEnableTimer(){
    this.gameService.setTimer(!this.isTimerEnabled);
  }

  getTimerSeconds(): number{
    return this.gameService.getTimerData();
  }
}
