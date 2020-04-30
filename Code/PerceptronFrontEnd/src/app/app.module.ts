import {MatIconModule, MatToolbarModule, MatRadioModule, MatPaginatorModule, MatTabsModule, MatSliderModule, MatCheckboxModule, MatTooltipModule, MatGridListModule, MatAutocompleteModule, MatSelectModule, MatExpansionModule, MatSidenavModule, MatCardModule, MatButtonModule, MatMenuModule, MatInputModule, MatSlideToggleModule, MatDialogModule} from '@angular/material';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule  }   from '@angular/forms';
import { HttpModule } from '@angular/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatTableModule} from '@angular/material/table';
import {HashLocationStrategy, LocationStrategy} from '@angular/common';
// import { ChartsModule } from 'ng2-charts';
import { FlashMessagesModule } from 'angular2-flash-messages';
import { EmbedVideo } from 'ngx-embed-video';
import { NgModule } from '@angular/core';
import 'hammerjs';
import { AppComponent } from './app.component';
import { AngularFireModule } from 'angularfire2';
import { AngularFireAuthModule } from 'angularfire2/auth';
import { environment } from '../environments/environment';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { EmailComponent } from './email/email.component';
import { LogedInComponent } from './loged-in/loged-in.component';
import { AuthGuard } from './auth.service';
import { routes } from './app.routes';
import { MasterComponent } from './master/master.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { ProteinSearchComponent } from './protein-search/protein-search.component';
import { AboutComponent } from './about/about.component';
import { BugComponent } from './bug/bug.component';
import { RepositoryComponent } from './repository/repository.component';
import { DemoComponent } from './demo/demo.component';
import { MapsComponent } from './maps/maps.component';
import { TestformComponent } from './testform/testform.component';
import { HudiaraComponent } from './hudiara/hudiara.component';
import { HistoryComponent } from './history/history.component';
import { HelpComponent } from './help/help.component';
import { SummaryResultsComponent } from './summary-results/summary-results.component';
import { DetailedResultsComponent } from './detailed-results/detailed-results.component';
import { ScanViewComponent } from './scan-view/scan-view.component';
import { SuggestionsComponent } from './suggestions/suggestions.component';
// import { PatterngeneratorComponent } from './patterngenerator/patterngenerator.component';
import { SpectralcountComponent } from './spectralcount/spectralcount.component';
import { XicComponent } from './xic/xic.component';
import { RegressionComponent } from './regression/regression.component';
import { HomeComponent } from './home/home.component';
import {MatProgressBarModule} from '@angular/material';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { TeamComponent } from './team/team.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { YoutubeComponent } from './youtube/youtube.component';
import { GettingStartedComponent } from './getting-started/getting-started.component';
import { ContactComponent } from './contact/contact.component';
import { SettingsComponent } from './settings/settings.component';
import { UpdatePasswordComponent } from './update-password/update-password.component';
import { UpdateUsernameComponent } from './update-username/update-username.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    EmailComponent,
    LogedInComponent,
    MasterComponent,
    ToolbarComponent,
    ProteinSearchComponent,
    AboutComponent,
    BugComponent,
    RepositoryComponent,
    DemoComponent,
    MapsComponent,
    TestformComponent,
    HudiaraComponent,
    HistoryComponent,
    HelpComponent,
    SummaryResultsComponent,
    DetailedResultsComponent,
    ScanViewComponent,
    SuggestionsComponent,
    // PatterngeneratorComponent,
    SpectralcountComponent,
    XicComponent,
    RegressionComponent,
    HomeComponent,
    TeamComponent,
    ResetPasswordComponent,
    YoutubeComponent,
    GettingStartedComponent,
    ContactComponent,
    SettingsComponent,
    UpdatePasswordComponent,
    UpdateUsernameComponent,
  ],
  imports: [
    MatIconModule,
    BrowserModule,
    MatRadioModule,
    MatTabsModule,
    MatTooltipModule,
    EmbedVideo.forRoot(),
    MatExpansionModule,
    MatSnackBarModule,
    MatSliderModule,
    MatPaginatorModule,
    MatSlideToggleModule,
    FlashMessagesModule.forRoot(),
    FormsModule,
    MatAutocompleteModule,
    MatGridListModule,
    MatCheckboxModule,
    MatSelectModule,
    ReactiveFormsModule ,
    MatProgressSpinnerModule,
    HttpModule,
    BrowserAnimationsModule,
    AngularFireModule.initializeApp(environment.firebase),
    AngularFireAuthModule,
    MatToolbarModule,
    MatSidenavModule,
    MatCardModule,
    MatButtonModule,
    MatMenuModule,
    MatInputModule,
    MatDialogModule,    
    MatTableModule, 
    // ChartsModule,
    MatProgressBarModule,
    routes
  ],
  providers: [
    AuthGuard,
    {provide:LocationStrategy, useClass:HashLocationStrategy}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
