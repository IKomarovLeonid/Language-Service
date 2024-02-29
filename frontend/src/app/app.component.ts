import {Component, OnInit} from '@angular/core';
import {PageViewModelOfWordModel, WordCategory, WordModel, WordsClient} from "../shared/main.api";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  wordsClient = new WordsClient();
  // words
  data: WordModel[] | undefined;
  filteredCollection: WordModel[] = [];
  wordToTranslate: WordModel | undefined;
  translation: string | undefined;
  correctAnswers: string[] | undefined;
  // selection
  selectedEnum: WordCategory = WordCategory.Any;
  // statistics
  wordsCount = 0;
  correctAnswersCount = 0;
  totalAnswers = 0;
  // for attempt history
  answers: UserAnswer[] = [];

    constructor() {
  }

  ngOnInit(): void {
      this.loadWords();
  }

  loadWords(){
      this.wordsClient.getWords()
        .then((data: PageViewModelOfWordModel) => {
          this.data = data.items;
          this.filteredCollection = data.items!!;
          this.wordsCount = data.items!!.length;
          this.showWord();
        })
        .catch(error => {
          alert(error);
        });
  }

  showWord() : void{
    if (this.filteredCollection && this.filteredCollection.length > 0) {
      const randomIndex = Math.floor(Math.random() * this.filteredCollection.length);
      this.wordToTranslate = this.filteredCollection[randomIndex];
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
        this.showWord();
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
        if(this.selectedEnum === WordCategory.Any){
          this.filteredCollection = this.data;
        }
        else {
          this.filteredCollection = this.data.filter(item => item.category === this.selectedEnum);
        }
      }
      this.wordsCount = this.filteredCollection.length;
      this.showWord();
  }

  finishAttempt(){
    this.reset();
    this.correctAnswersCount = 0;
    this.totalAnswers = 0;
    this.translation = undefined;
    this.showWord();
    console.log(this.answers);
    this.answers = [];
  }

  saveAttempt(word: string, expectedTranslation: string[], userAnswer: string, isCorrect : boolean){
    const answer: UserAnswer = {
      word: word,
      expectTranslation: expectedTranslation,
      userAnswer: userAnswer,
      isCorrect: isCorrect
    };
    this.answers.push(answer);
  }

  protected readonly Object = Object;
  protected readonly WordCategory = WordCategory;

}

interface UserAnswer {
  word: string;
  expectTranslation: string[];
  userAnswer: string;
  isCorrect: boolean;
}
