import {Component, OnInit} from '@angular/core';
import {AttemptHistoryModel, AttemptModel, WordCategory, WordModel, WordType} from "../shared/main.api";
import {ApiClient} from "../services/api.client";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  data: WordModel[] | undefined;
  history: AttemptHistoryModel[] | undefined;
  filteredCollection: WordModel[] = [];
  wordToTranslate: WordModel | undefined;
  translation: string | undefined;
  correctAnswers: string[] | undefined;
  // selection
  selectedEnumCategory: WordCategory = WordCategory.Any;
  selectedEnumType: WordType = WordType.Any;
  // statistics
  wordsCount = 0;
  correctAnswersCount = 0;
  totalAnswers = 0;
  // for attempt history
  answers: AttemptModel[] = [];
  // toggles
  isRepeatErrors = false;

    constructor(private client : ApiClient) {
  }

  ngOnInit(): void {
      this.loadWords();
      this.loadHistory();
  }

  async loadWords(){
      let apiResult = await this.client.getWords();
      if(apiResult){
        this.data = apiResult.items!!;
        this.filteredCollection = apiResult.items!!;
        this.wordsCount = this.filteredCollection.length;
        this.showWord();
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

  showWord() : void{
    if (this.filteredCollection && this.filteredCollection.length > 0) {
      const randomIndex = Math.floor(Math.random() * this.filteredCollection.length);
      this.wordToTranslate = this.filteredCollection[randomIndex];
      if(this.wordToTranslate.word === undefined || this.wordToTranslate.translations === undefined){
        alert('Bad word no translations' + this.wordToTranslate);
      }
    }
  }

  reset(){
      this.wordToTranslate = undefined;
      this.translation = undefined;
  }

  makeAnswer(){
      // handle user no input
      if(this.translation === undefined || this.translation.trim() === ''){
        this.totalAnswers ++;
        this.correctAnswers = this.wordToTranslate?.translations;
        this.saveAttempt(
          this.wordToTranslate?.word!!,
          this.correctAnswers!!, this.translation ?? 'N/A', false);
        return;
      }

      this.correctAnswers = this.wordToTranslate?.translations;
      let filtered = this.correctAnswers?.filter(w => w == this.translation?.toLowerCase())
      if(filtered!!.length > 0){
        this.correctAnswersCount ++;
        this.totalAnswers ++;
        this.saveAttempt(
          this.wordToTranslate?.word!!,
          this.correctAnswers!!, this.translation, true);
        this.correctAnswers = undefined;
      }
      else {
        this.totalAnswers ++;
        this.saveAttempt(
          this.wordToTranslate?.word!!,
          this.correctAnswers!!, this.translation, false);
      }
      this.reset();
      this.showWord();
  }

  filterWords(){
      if(this.data){
        if(this.selectedEnumCategory === WordCategory.Any && this.selectedEnumType === WordType.Any){
          this.filteredCollection = this.data;
        }
        else {
          if(this.selectedEnumCategory === WordCategory.Any){
            this.filteredCollection = this.data.filter(item => item.type === this.selectedEnumType);
          }
          else{
            if(this.selectedEnumType === WordType.Any){
              this.filteredCollection = this.data.filter(item => item.category === this.selectedEnumCategory);
            }
            else this.filteredCollection = this.data.filter(item =>
              item.category === this.selectedEnumCategory &&
              item.type === this.selectedEnumType
            );
          }
        }
      }
      this.wordsCount = this.filteredCollection.length;
      this.showWord();
  }

  async finishAttempt(){
    await this.client.createAttempt(
      this.answers,
      this.correctAnswersCount, 30,
      this.selectedEnumType,
      this.selectedEnumCategory);
    this.reset();
    this.correctAnswersCount = 0;
    this.totalAnswers = 0;
    this.translation = undefined;
    this.answers = [];
    this.showWord();
    this.loadHistory();
  }

  showHistoryInfo(attempt: AttemptHistoryModel){
    let combinedString = Object.entries(attempt.errors!!)
      .map(([key, value]) => `${key}: ${value}`)
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

  onRepeatErrors(){
      this.isRepeatErrors = !this.isRepeatErrors;
      console.log(this.isRepeatErrors);
  }

  protected readonly Category = Object;
  protected readonly WordCategory = WordCategory;

  protected readonly Type = Object;
  protected readonly WordType = WordType;
}
