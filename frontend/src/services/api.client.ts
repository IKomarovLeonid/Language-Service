import {Injectable} from "@angular/core";
import {
  CreateGameResultRequestModel,
  GameClient,
  UsersClient,
  WordsClient,
} from "../shared/main.api";

@Injectable({"providedIn": 'root'})
export class ApiClient{
  words: WordsClient;
  users: UsersClient;
  games: GameClient;

  constructor(){
    this.words = new WordsClient();
    this.users = new UsersClient();
    this.games = new GameClient();
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

  async getGames(userId: number){
    try{
      return await this.games.getGames(userId);
    }
    catch{
      alert('Api error getting games');
      return undefined;
    }
  }

  async getUser(userId: number){
    try{
      return await this.users.getUsers(userId);
    }
    catch{
      alert('Api error getting user');
      return undefined;
    }
  }

  async getStatistics(userId: number){
    try{
      return await this.words.getWordStatistics(userId);
    }
    catch{
      alert('Api error getting statistics');
      return undefined;
    }
  }

  async createGameResult(request: CreateGameResultRequestModel){
    try{
      return await this.games.createGameResult(request);
    }
    catch{
      alert('Api error creation game');
      return undefined;
    }
  }

}
