import {Component, Input, OnInit} from '@angular/core';
import {AttemptHistoryModel, AttemptModel, LanguageType, WordCategory, WordModel, WordType} from "../shared/main.api";
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
  @Input() selectedEnumCategory = WordCategory.Any;
  @Input() selectedEnumType = WordType.Any;
  @Input() enumLanguage: LanguageType = LanguageType.SpanishRussian;
  @Input() enumCategoryValues: WordCategory[] = Object.values(WordCategory);
  @Input() enumTypeValues: WordType[] = Object.values(WordType);
  @Input() enumLanguageValues: LanguageType[] = Object.values(LanguageType);

  // for attempt history
  history: AttemptHistoryModel[] | undefined;
  answers: AttemptModel[] = [];
  // toggles
  isRepeatWords = true;
  isLanguageReversed = false;
  isTimerEnabled = false;
  isConjugation = false;
  // message
  userShowMessage: string | undefined;

  languageTypeMapping = {
    [LanguageType.SpanishRussian]: 'Spanish <-> Russian',
    [LanguageType.EnglishRussian]: 'English <-> Russian',
  };

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
        this.gameService.setWords(apiResult.items!!.filter(item => item.language === this.enumLanguage));
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
          this.gameService.setWords(this.wordsFromServer.filter(item => item.language === this.enumLanguage));
        }
        else {
          this.gameService.setWords(this.wordsFromServer.filter(
            item => item.type === this.selectedEnumType &&
              item.category === this.selectedEnumCategory &&
              item.language === this.enumLanguage
          ));
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
    this.gameService.resetTime();
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
      model.totalSeconds = this.gameService.getInitialSeconds() - this.gameService.getTimerSecondsLeft();
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
        let errors = model.errors;
        if(errors){
          let len = Object.keys(errors).length;
          if (len <= 0) {
            alert('No words to repeat from this history');
            return;
          }
          let words = new Array<WordModel>();
          for (const p in errors) {
            if (errors.hasOwnProperty(p)) {
              if(this.wordsFromServer){
                let word = this.wordsFromServer.filter(w => w.word === p);
                if(word.length > 0) words.push(word[0]);
                else{
                  let filtered = this.wordsFromServer.filter(w =>
                    w.translations!!.filter(w =>
                      w === p).length > 0);
                  if(filtered.length > 0) words.push(filtered[0]);
                }
              }
            }
          }
          this.gameService.finish();
          this.gameService.setWords(words);
          this.setWord();

        }
      }
      else alert('This history has undefined id')
  }

  onConjugation(){
    if(!this.isConjugation){
      let words = this.wordsFromServer!!.filter(w => w.conjugation != undefined);
      this.gameService.setWords(words);
      this.setWord();
    }
    else{
      this.filterWords();
    }
  }
}
