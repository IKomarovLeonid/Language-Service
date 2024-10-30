import {ApiClient} from "./api.client";
import {AttemptHistoryModel} from "../shared/main.api";
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
      console.log(apiResult.items!!)
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

  public async createHistory(totalAttempts: number, correctAnswers: number, errors: string){
    await this.client.createAttempt(totalAttempts, correctAnswers, errors);
    await this.loadHistory();
  }
}
