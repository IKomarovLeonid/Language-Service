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

  showHistoryInfo(attempt: AttemptHistoryModel){
    if(attempt.wordErrors){
      console.log(attempt.wordErrors);
      let errors = attempt.wordErrors.split(",");
      alert("Errors was: " + errors.join("\n"));
    }
    else {
      alert("This attempt has no word errors")
    }
  }

  public hasItems(){
    if(this.history){
      return this.history.length > 0;
    }
    return false;
  }
}

