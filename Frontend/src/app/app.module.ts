import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule} from "@angular/common/http";
import { ResultComponent } from './result/result.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { ForceDirectedGraphComponent } from './force-directed-graph/force-directed-graph.component';
import { ForceGraphCytoscapeComponent } from './force-graph-cytoscape/force-graph-cytoscape.component';

@NgModule({
  declarations: [
    AppComponent,
    ResultComponent,
    ForceDirectedGraphComponent,

    ForceGraphCytoscapeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,

  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
