import {Component, OnInit} from '@angular/core';
import {Language, WordsClient, WordsType} from "../api/ApiClient";
import {ApiService} from "../api/ApiService";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  wordToTranslate: string;
  expectedTranslation: string;
  comparisonMessage: string = '';
  translation: string = '';
  targetCounter: number = 1;
  correctAnswers: number = 0;
  wordsType: WordsType =  WordsType.All;
  words: string[];
  translations: string[];
  gameCompleted: boolean = false;
  totalAttempts: number = 0;
  gameCompletionDate: Date | undefined;
  buttonTitle = 'Check';

  constructor(private client: ApiService) {
    this.words = [];
    this.translations = [];
    this.wordToTranslate = '';
    this.expectedTranslation = '';
  }

  ngOnInit(): void {
    this.loadWords(this.wordsType);
  }

  checkTranslation() {
    if(this.gameCompleted) {
      this.startNewGame();
      return;
    }
    this.totalAttempts++;
    const expectedTranslation = this.expectedTranslation;

    if (this.translation.trim().toLowerCase() === expectedTranslation.toLowerCase()) {
      this.correctAnswers++;
      this.comparisonMessage = "Correct!"
    }
    else this.comparisonMessage = `Answer of ${this.wordToTranslate} is wrong, correct is: ${this.expectedTranslation}`;

    if (this.correctAnswers < this.targetCounter) {
      this.showRandomWord();
    } else {
      // Set game completion date
      this.gameCompletionDate = new Date();
      this.buttonTitle = 'Start';
      // Display the game completion message
      this.gameCompleted = true;

      // Clear input and reset the correct counter
      this.translation = '';
    }
  }

  loadWords(type: WordsType){
    // fetch from server side
    this.client.words.get(
      Language.Portugues,
      Language.Russian,
      type).then(response => {
        if(response.words){
          this.words = Object.keys(response.words);
          this.translations = Object.values(response.words);
          this.startNewGame();
        }
    }).catch(error => {
       alert("Server api side disconnected")
    });
  }

  showRandomWord() {
    const randomIndex = Math.floor(Math.random() * this.words.length);
    this.wordToTranslate = this.words[randomIndex];
    this.expectedTranslation = this.translations[randomIndex];
    this.translation = '';
  }

  calculatePercentage() {
    return ((this.correctAnswers / this.totalAttempts) * 100).toFixed(2);
  }

  startNewGame() {
    // Reset the game stats and start a new game
    this.totalAttempts = 0;
    this.correctAnswers = 0;
    this.gameCompleted = false;
    this.buttonTitle = 'Check';
    this.comparisonMessage = '';
    this.showRandomWord();
  }

  protected readonly WordsType = WordsType;

  onSelectClick(){
    // do nothing if current type not changed

    this.loadWords(this.wordsType);
  }
}
