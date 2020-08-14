import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';
import { Chart } from 'chart.js';
import Hammer from 'hammerjs';
import { max } from 'rxjs/operator/max';

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
  PeakListIntensities: number[] = [];
  PeakListMasses: number[] = [];
  Experimentalmz: number[] = [];
  Theoreticalmz: number[] = [];
  LabelsArray: string[] = [];

  chart = [];

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
    for (let i = 0; i < data.InsilicoSpectra.ListIndices.length; i++) {
      this.Experimentalmz.push(data.InsilicoSpectra.ListExperimental_mz[i]);
      this.Theoreticalmz.push(data.InsilicoSpectra.ListTheoretical_mz[i]);
    }
    for(let j = 0; j<data.PeakListData.length; j++)
    {
      this.PeakListMasses.push(data.PeakListData[j].Mass);
      this.PeakListIntensities.push(data.PeakListData[j].Intensity);
    }


    this.dataSource = new MatTableDataSource(users);
    this.base64data = data.blob;
    this.ImageFilePath = this.sanitizer.bypassSecurityTrustUrl('data:image/jpg;base64,' + this.base64data);

    //this.array = data.PeakListMasses.split(",",100);
    //Data Preparation for Graph
    // this.PeakListMasses = data.PeakListData.PeakListMasses.split(',').map(Number);
    // this.PeakListIntensities = data.PeakListData.PeakListIntensities.split(',').map(Number);
    //this.LabelsArray = data.PeakListData.PeakListMasses;
    // var maxPeakListMass = Math.max(this.PeakListMasses);
    // var minPeakListMass = Math.min(this.PeakListMasses);
    // var PeakListMassHalf = maxPeakListMass/2;
    // var PeakListMassOneFourth = maxPeakListMass * 0.25;
    // var PeakListMassThreeFourth = maxPeakListMass * 0.75;
    this.LabelsArray.push(Math.min.apply(Math, this.PeakListMasses).toString());
    this.LabelsArray.push((Math.max.apply(Math, this.PeakListMasses) * 0.25).toString());
    this.LabelsArray.push((Math.max.apply(Math, this.PeakListMasses) * .50).toString());
    this.LabelsArray.push((Math.max.apply(Math, this.PeakListMasses) * .75).toString());
    this.LabelsArray.push(Math.max.apply(Math, this.PeakListMasses).toString());

    let as = Math.max.apply(Math, this.PeakListMasses);
    //let v = Math.min(this.PeakListMasses);
//     let minDataValue = Math.min(Math.min(this.PeakListMasses), options.ticks.suggestedMin);
// let maxDataValue = Math.max(Math.max(this.PeakListMasses), options.ticks.suggestedMax);

this.chart = new Chart('canvas', {
  type: 'bar',
  options: {
    scales: {
        xAxes: [{
            ticks: {
                display: false //this will remove only the label
            },
            gridLines: {
              display: false,
            }
        }]
    }
},

  data: {
    labels:  this.PeakListMasses,
    // type: 'column',
    datasets: [{
      label: "Experimental Mass",
      data: [1], //this.Experimentalmz,
      barThickness: 1,
      backgroundColor: 'rgba(255, 0, 0, 1.0)'
    },
    {
      label: "Peak List Masses",
      data: this.PeakListIntensities,
      barThickness: 1,
      backgroundColor: 'rgba(0, 0, 0, 1.0)'
    },
    {
      label : "Theoretical Mass",
      data: [1], //this.Theoreticalmz,
      barThickness: 1,
      backgroundColor: 'rgba(0, 128, 0, 1.0)'
    }]
  }
})

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