import {Component, OnDestroy} from '@angular/core';
import {GameComponent} from "./game/game.component";
import {StatisticsComponent} from "./statistics/statistics.component";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  currentComponent: any;

  constructor() {
    this.currentComponent = GameComponent;
  }

  changeContent(menu: string) {
    switch(menu) {
      case 'game':
        this.currentComponent = GameComponent;
        break;
      case 'statistics':
        this.currentComponent = StatisticsComponent;
        break;
      default:
        this.currentComponent = GameComponent;
    }
  }
}
