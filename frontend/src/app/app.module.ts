import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import {FormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import { ConjugationComponentComponent } from './conjugation-component/conjugation-component.component';
import { WordInfoComponentComponent } from './word-info-component/word-info-component.component';
import { TogglesComponentComponent } from './toggles-component/toggles-component.component';
import { HistoryComponentComponent } from './history-component/history-component.component';
@NgModule({
  declarations: [
    AppComponent,
    ConjugationComponentComponent,
    WordInfoComponentComponent,
    WordInfoComponentComponent,
    TogglesComponentComponent,
    HistoryComponentComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
