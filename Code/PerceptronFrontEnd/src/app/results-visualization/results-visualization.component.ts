import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-results-visualization',
  templateUrl: './results-visualization.component.html',
  styleUrls: ['./results-visualization.component.css'],
  providers: [ConfigService]
})
export class ResultsVisualizationComponent implements OnInit {
  displayedColumns = ['rank', 'FragmentID', 'FragmentIon', 'ExperimentalMZ', 'TheoreticalMZ', 'MassDifference'];
  dataSource: MatTableDataSource<UserData>;
  resultId: any;
  ImageFilePath: any;
  base64data: any;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private route: ActivatedRoute, private _httpService: ConfigService, private sanitizer: DomSanitizer) {
    const users: UserData[] = [];
    this.dataSource = new MatTableDataSource(users);
  }
  ngOnInit() {
    this.route.params.subscribe((params: Params) => this.resultId = params['resultId']);
    this._httpService.GetDetailedProteinHitViewResults(this.resultId).subscribe(data => this.what(data));
  }
  what(data: any) {
    const users: UserData[] = [];
    for (let i = 0; i < data.InsilicoSpectra.ListIndices.length; i++) {
      users.push(createNewUser(i + 1,
        data.InsilicoSpectra.ListIndices[i].toString(),
        data.InsilicoSpectra.ListFragIon[i].toString(),
        data.InsilicoSpectra.ListExperimental_mz[i].toString(),
        data.InsilicoSpectra.ListTheoretical_mz[i].toString(),
        data.InsilicoSpectra.ListAbsError[i].toString()
      ));
    }
    this.dataSource = new MatTableDataSource(users);
    this.base64data = data.blob;
    this.ImageFilePath = this.sanitizer.bypassSecurityTrustUrl('data:image/jpg;base64,' + this.base64data);
  }
}


/** Builds and returns a new User. */
function createNewUser(id: number, index: string, FragIon: string, ExpMZ: string, ThrMZ: string, AbsError: string): UserData {
  return {
    rank: id.toString(),
    FragmentID: index,
    FragmentIon: FragIon,
    ExperimentalMZ: ExpMZ,
    TheoreticalMZ: ThrMZ,
    MassDifference: AbsError
  };
}
export interface UserData {
  rank: string;
  FragmentID: string;
  FragmentIon: string;
  ExperimentalMZ: string;
  TheoreticalMZ: string;
  MassDifference: string;
}