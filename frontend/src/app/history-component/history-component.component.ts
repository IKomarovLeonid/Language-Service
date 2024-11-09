import {Component, OnDestroy, OnInit} from '@angular/core';
import {GameService} from "../../services/game.service";
import {AttemptHistoryModel} from "../../shared/main.api";
import {HistoryService} from "../../services/history.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-history-component',
  templateUrl: './history-component.component.html',
  styleUrls: ['./history-component.component.css']
})
export class HistoryComponentComponent implements OnInit, OnDestroy{

  history: AttemptHistoryModel[] | null | undefined;
  private dataSubscription: Subscription;

  constructor(private service: HistoryService, private gameService: GameService) {
    this.dataSubscription = this.service.historyData$.subscribe(value => {
      this.history = value;
    });
  }
  ngOnInit(): void {
    this.service.loadHistory();
  }
  ngOnDestroy() {
    this.dataSubscription.unsubscribe();
  }

  onDeleteHistory(id: number | undefined){
    this.service.deleteHistory(id!!);
    this.service.loadHistory();
  }

  onRepeatHistory(id: number | undefined){
    if(id){
      let model = this.service.getModel(id);
      if(model){
        this.gameService.repeatWords(model.wordErrors);
        this.gameService.setAnyWord();
      }
      else  alert(`Unknown history ${id}`);
    }
    else alert('This history has undefined id')
  }

  public hasItems(){
    if(this.history){
      return this.history.length > 0;
    }
    return false;
  }
}

