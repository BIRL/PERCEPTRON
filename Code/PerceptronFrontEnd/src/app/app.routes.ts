import { RepositoryComponent } from './repository/repository.component';
import { HudiaraComponent } from './hudiara/hudiara.component';
import { BugComponent } from './bug/bug.component';
import { HelpComponent } from './help/help.component';
import { HistoryComponent } from './history/history.component';
import { FdrComponent } from './fdr/fdr.component';
import { DemoComponent } from './demo/demo.component';
import { AboutComponent } from './about/about.component';
import { TestformComponent } from './testform/testform.component';
import { MapsComponent } from './maps/maps.component';
import { HomeComponent } from './home/home.component';
import { ProteinSearchComponent } from './protein-search/protein-search.component';
import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ContactComponent } from './contact/contact.component';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { TeamComponent } from './team/team.component';
import { LogedInComponent } from './loged-in/loged-in.component';
import { AuthGuard } from './auth.service';
import { SignupComponent } from './signup/signup.component';
import { EmailComponent } from './email/email.component';
import { SummaryResultsComponent } from './summary-results/summary-results.component';
import { DetailedResultsComponent } from './detailed-results/detailed-results.component';
import { ResultsVisualizationComponent } from './results-visualization/results-visualization.component';
import { ScanViewComponent } from './scan-view/scan-view.component';
import { SuggestionsComponent } from './suggestions/suggestions.component';
import { ResultsDownloadComponent } from './results-download/results-download.component';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
// import { PatterngeneratorComponent } from './patterngenerator/patterngenerator.component';
// import { HomeComponent } from './home/home.component';

import { SpectralcountComponent } from './spectralcount/spectralcount.component';
import { XicComponent } from './xic/xic.component';
import { RegressionComponent } from './regression/regression.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { YoutubeComponent } from './youtube/youtube.component';
import { GettingStartedComponent } from './getting-started/getting-started.component';
import { SettingsComponent } from './settings/settings.component';
import { UpdatePasswordComponent } from './update-password/update-password.component';
import { UpdateUsernameComponent } from './update-username/update-username.component';
export const router: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'signup', component: SignupComponent },
    { path: 'youtube', component: YoutubeComponent, canActivate: [AuthGuard]  },
    { path: 'getting', component: GettingStartedComponent}, // There will be no need to Login
    // { path: 'getting', component: GettingStartedComponent, canActivate: [AuthGuard]  },
    { path: 'login-email', component: EmailComponent },
    { path: 'reset', component: ResetPasswordComponent},
    { path: 'search', component: ProteinSearchComponent, canActivate: [AuthGuard] },
    { path: 'update-password', component: UpdatePasswordComponent, canActivate: [AuthGuard] },
    { path: 'update-username', component: UpdateUsernameComponent, canActivate: [AuthGuard] },
    { path: 'yoohoo', component: LogedInComponent, canActivate: [AuthGuard] },
    { path: 'about', component: AboutComponent },
    { path: 'contact', component: ContactComponent},
    { path: 'settings', component: SettingsComponent, canActivate: [AuthGuard] },
    { path: 'home', component: HomeComponent },
    { path: 'bug-report', component: BugComponent,  canActivate: [AuthGuard] },
    { path: 'repository', component: RepositoryComponent, canActivate: [AuthGuard] },
    { path: 'maps', component: MapsComponent, canActivate: [AuthGuard] },
    { path: 'hudiara', component: HudiaraComponent, canActivate: [AuthGuard]},
    { path: 'demo', component: DemoComponent,  canActivate: [AuthGuard]},
    { path: 'team', component: TeamComponent,  canActivate: [AuthGuard]},
    { path: 'history', component: HistoryComponent, canActivate: [AuthGuard]},
    { path: 'fdr', component: FdrComponent, canActivate: [AuthGuard]},
    { path: 'help', component: HelpComponent, canActivate: [AuthGuard]},
    { path: 'suggest', component: SuggestionsComponent, canActivate: [AuthGuard]},
    { path: 'summaryresults/:querryId/:fileID', component: SummaryResultsComponent},
    { path: 'detailedresults/:resultId/:rank', component: DetailedResultsComponent},
    { path: 'resultsdownload/:querryId', component: ResultsDownloadComponent},
    { path: 'adminpanel' , component: AdminPanelComponent},
    //    children: [{path: 'results-visualization', component: ResultsVisualizationComponent}]},

    // { path: 'patterngenerator', component: PatterngeneratorComponent, canActivate: [AuthGuard]},
    { path: 'resultsvisualization/:resultId/:rank', component: ResultsVisualizationComponent},
    { path: 'scans/:querryId', component: ScanViewComponent},
    { path: 'sc/:querryId', component: SpectralcountComponent},
    { path: 'xic/:querryId', component: XicComponent},
    { path: 'reg/:querryId', component: RegressionComponent},
]

export const routes: ModuleWithProviders = RouterModule.forRoot(router);