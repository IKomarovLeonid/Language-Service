import {ApiClient} from "./api.client";
import {AttemptHistoryModel, AttemptModel, WordCategory, WordType} from "../shared/main.api";
import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";

@Injectable({"providedIn": 'root'})
export class HistoryService{

  private history = new BehaviorSubject<AttemptHistoryModel[] | null>(null);

  constructor(private client: ApiClient) {
  }

  public async loadHistory(){
    let apiResult = await this.client.getHistory();
    if(apiResult){
      this.history.next(apiResult.items!!);
    }
    else alert('Unable to fetch history from server');
  }

  public getModel(id: number | undefined){
    if(id){
      return this.history.getValue()!!.filter(h => h.id === id)[0]!!;
    }
    return undefined;
  }

  public async deleteHistory(id: number){
    await this.client.deleteHistory(id);
    await this.loadHistory();
  }

  public get historyData$() {
    return this.history.asObservable();
  }

  public async createHistory(attempts: AttemptModel[], wordType: WordType, wordCategory: WordCategory, correctAnswers: number){
    await this.client.createAttempt(
      attempts,
      correctAnswers, 30,
      wordType,
      wordCategory);
    await this.loadHistory();
  }
}
