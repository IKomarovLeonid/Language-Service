import {Component, OnInit} from '@angular/core';
import {GameService} from "../../services/game.service";
import {ApiClient} from "../../services/api.client";
import {AttemptHistoryModel} from "../../shared/main.api";

@Component({
  selector: 'app-history-component',
  templateUrl: './history-component.component.html',
  styleUrls: ['./history-component.component.css']
})
export class HistoryComponentComponent implements OnInit{

  history: AttemptHistoryModel[] | undefined;

  constructor(private gameService: GameService, private client: ApiClient) {
  }

  async onDeleteHistory(id: number | undefined){
    let result = await this.client.deleteHistory(id!!);
    if(!result) alert('Failed to delete history attempt');
    this.loadHistory();
  }

  onRepeatHistory(id: number | undefined){
    if(id){
      let model = this.history?.filter(h => h.id === id)[0]!!;
      this.gameService.setRetryWords(model);
      this.gameService.setAnyWord();
    }
    else alert('This history has undefined id')
  }

  showHistoryInfo(attempt: AttemptHistoryModel){
    let combinedString = Object.entries(attempt.errors!!)
      .map(([key, value]) => `'${key}' errors was '${value}' times`)
      .join('\n');
    alert(combinedString);
  }

  ngOnInit(): void {
    this.loadHistory();
  }

  private async loadHistory(){
    let apiResult = await this.client.getHistory();
    if(apiResult){
      this.history = apiResult.items!!;
    }
    else alert('Unable to fetch history from server');
  }

  public hasItems(){
    if(this.history){
      return this.history.length > 0;
    }
    return false;
  }
}

