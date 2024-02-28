import {Component, OnInit} from '@angular/core';
import {PageViewModelOfWordModel, WordCategory, WordModel, WordsClient} from "../shared/main.api";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  client : WordsClient;
  data: WordModel[] | undefined;
  wordToTranslate: WordModel | undefined;
  translation: string | undefined;
  correctAnswer: string | undefined;
  correctAnswers = 0;
  totalAnswers = 0;

    constructor() {
    this.client = new WordsClient();
  }

  ngOnInit(): void {
      this.loadWords();
  }

  loadWords(){
    this.client.getWords()
      .then((data: PageViewModelOfWordModel) => {
        this.data = data.items?.filter(w => w.category === WordCategory.Colors);
        this.showWord();
      })
      .catch(error => {
        alert(error);
      });
  }

  showWord() : void{
    if (this.data && this.data.length > 0) {
      const randomIndex = Math.floor(Math.random() * this.data.length);
      this.wordToTranslate = this.data[randomIndex];
    }
  }

  reset(){
      this.wordToTranslate = undefined;
      this.translation = undefined;
  }

  makeAnswer(){
      this.correctAnswer = undefined;
      if(this.translation){
        this.correctAnswer = this.wordToTranslate?.translation;
        if(this.translation.toLowerCase() == this.correctAnswer){
          this.correctAnswers ++;
          this.totalAnswers ++;
          this.correctAnswer = undefined;
        }
        else {
          this.totalAnswers ++;
        }
      }
      this.reset();
      this.showWord();
  }

}
