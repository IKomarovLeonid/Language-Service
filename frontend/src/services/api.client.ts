import {Injectable} from "@angular/core";
import {
  AttemptModel,
  CreateAttemptHistoryRequestModel,
  HistoryClient,
  WordCategory,
  WordsClient,
  WordType
} from "../shared/main.api";

@Injectable({"providedIn": 'root'})
export class ApiClient{
  words: WordsClient;
  history: HistoryClient;

  constructor(){
    this.words = new WordsClient();
    this.history = new HistoryClient();
  }

  async getWords(){
    try{
      return await this.words.getWords();
    }
    catch{
      alert('Api error getting words');
      return undefined;
    }
  }

  async getHistory(){
    try{
      return await this.history.getAttempts();
    }
    catch{
      alert('Api error getting history');
      return undefined;
    }
  }

  async createAttempt(attempts: AttemptModel[], correctAnswers: number, totalSeconds: number, types: WordType, category: WordCategory){
    try{
      var request = new CreateAttemptHistoryRequestModel();
      request.attempts = attempts;
      request.correctAttempts = correctAnswers;
      request.totalAttempts = attempts.length;
      request.totalSeconds = totalSeconds;
      request.category = category;
      request.wordTypes = types
      this.history.createAttemptHistory(request);
    }
    catch{
      alert('Api error creating history');
      return undefined;
    }
  }

  async deleteHistory(id: number){
    try{
      return await this.history.deleteAttemptHistory(id);
    }
    catch{
      alert('Api error removing history');
      return undefined;
    }
  }
}
