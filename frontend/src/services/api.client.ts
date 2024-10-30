import {Injectable} from "@angular/core";
import {
  CreateAttemptHistoryRequestModel,
  HistoryClient,
  WordsClient,
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
      return await this.words.getWords(null);
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

  async createAttempt(totalAttempts: number, correctAnswers: number, errors: string){
    try{
      let request = new CreateAttemptHistoryRequestModel();
      request.correctAttempts = correctAnswers;
      request.totalAttempts = totalAttempts;
      request.userId = 1;
      request.wordErrors = errors;
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
