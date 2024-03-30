import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import {FormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import { ConjugationComponentComponent } from './conjugation-component/conjugation-component.component';
import { WordInfoComponentComponent } from './word-info-component/word-info-component.component';
@NgModule({
  declarations: [
    AppComponent,
    ConjugationComponentComponent,
    WordInfoComponentComponent,
    WordInfoComponentComponent
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
