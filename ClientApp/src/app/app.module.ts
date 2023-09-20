import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { SolverComponent } from './solver/solver.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material-module';
import { SimplexStepsComponent } from './visualisation/simplex/simplex-steps.component';
import { VisualisationComponent } from './visualisation/visualisation.component';
import { SanitizedHtmlPipe } from '../pipes/sanitizer-pipe';
import { SimplexTableComponent } from './visualisation/simplex/simplex-table.component';
import { HelpComponent } from './help/help.component';
import { MWUMStepsComponent } from './visualisation/mwum/mwum-steps.component';
import { MWUMTableComponent } from './visualisation/mwum/mwum-table.component';
import { MWUMSummaryTableComponent } from './visualisation/mwum/mwum-summary-table.component';
import { EfficiencyComponent } from './efficiency/efficiency.component';





@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SolverComponent,
    SimplexStepsComponent,
    VisualisationComponent,
    SanitizedHtmlPipe,
    SimplexTableComponent,
    HelpComponent,
    MWUMStepsComponent,
    MWUMTableComponent,
    MWUMSummaryTableComponent,
    EfficiencyComponent,
  ],
  imports: [
    MaterialModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'visualisation/:option', component: VisualisationComponent },
      { path: 'help', component: HelpComponent },
      { path: 'efficiency', component: EfficiencyComponent },
    ]),
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
