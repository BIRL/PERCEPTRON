import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';
import * as CanvasJS from 'canvasjs-non-commercial-2.3.2/canvasjs.min';

@Component({
  selector: 'app-results-visualization',
  templateUrl: './results-visualization.component.html',
  styleUrls: ['./results-visualization.component.css'],
  providers: [ConfigService]
})
export class ResultsVisualizationComponent implements OnInit {
  displayedColumns = ['rank', 'FragmentID', 'FragmentIon', 'ExperimentalMZ', 'TheoreticalMZ', 'MassDifference'];
  dataSource: MatTableDataSource<UserData>;
  querryId: any;
  resultId: any;
  rank: any;
  ImageFilePath: any;
  base64data: any;
  PeakListIntensities: number[] = [];
  PeakListMasses: number[] = [];
  Experimentalmz: number[] = [];
  Theoreticalmz: number[] = [];
  LabelsArray: string[] = [];

  dataPointsPeakList: any;// number[] = [];//this.dataPoints1 = [];
  dataPointsExperimentalmz:any;// number[] = [];//this.dataPoints2 = [];
  dataPointsTheoreticalmz:any;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private route: ActivatedRoute, private _httpService: ConfigService, private sanitizer: DomSanitizer) {
    const users: UserData[] = [];
    this.dataSource = new MatTableDataSource(users);
  }
  ngOnInit() {
    this.route.params.subscribe((params: Params) => this.querryId = params['querryId']);
    this.route.params.subscribe((params: Params) => this.resultId = params['resultId']);
    this.route.params.subscribe((params: Params) => this.rank = params['rank']);
    // this.route.params.subscribe((params: Params) => this.resultId = params['resultId']);
    this._httpService.GetDetailedProteinHitViewResults(this.querryId,this.resultId,this.rank).subscribe(data => this.what(data));
  }
  ngAfterViewInit() { //Added //Updated 20201215 
    // Scrolls to top of Page after page view initialized
    let top = document.getElementById('top');
    if (top !== null) {
      top.scrollIntoView();
      top = null;
    }
  }
  // ngAfterViewInit() { //Added //Updated 20201215 
  //   // Scrolls to top of Page after page view initialized
  //   let top = document.getElementById('top');
  //   if (top !== null) {
  //     top.scrollIntoView();
  //     top = null;
  //   }
  // }
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

    this.LabelsArray.push(Math.min.apply(Math, this.PeakListMasses).toString());
    this.LabelsArray.push((Math.max.apply(Math, this.PeakListMasses) * 0.25).toString());
    this.LabelsArray.push((Math.max.apply(Math, this.PeakListMasses) * .50).toString());
    this.LabelsArray.push((Math.max.apply(Math, this.PeakListMasses) * .75).toString());
    this.LabelsArray.push(Math.max.apply(Math, this.PeakListMasses).toString());

    let as = Math.max.apply(Math, this.PeakListMasses);

    this.dataPointsPeakList = [];   //dataPoints1
    this.dataPointsExperimentalmz = [];          //dataPoints2
    this.dataPointsTheoreticalmz = [];          //dataPoints3

    for (var i = 0; i < this.PeakListMasses.length; i++) {

      //let xvalue: any = this.x1[i];
      this.dataPointsPeakList.push({
        x:this.PeakListMasses[i],   //x1
        y: this.PeakListIntensities[i] //y1
      });
    }
    for (var i = 0; i < this.Experimentalmz.length; i++){
      this.dataPointsExperimentalmz.push({
        x: this.Experimentalmz[i],   //x2
        y: 1
      });
    }
    for (var i = 0; i < this.Theoreticalmz.length; i++){
      this.dataPointsTheoreticalmz.push({
        x: this.Theoreticalmz[i],  //x3
        y: 1
      });
    }
    let chart = new CanvasJS.Chart("chartContainer", {
      // title: {
      //   text: "Basic Column Chart in Angular"
      // },
      // width:550,
      dataPointMaxWidth: 1.5,
      zoomEnabled: true,
      axisY:{
        title:'Normalized Relative Abundance',
        gridThickness: 0,
        titleFontSize: 16,
      },
      axisX:{
        title: 'm/z',
        minimum: 0,
        tickLength: 8,
        titleFontSize: 16,
        valueFormatString: '#######',
      },
      toolTip:{
        content: '{name} <br/> x: {x}, y: {y}',
      },
      data: [{
        type: "column",
        color: 'black',
        name: 'PeakListMasses',
        showInLegend: true,
        legendText: 'Peak List Masses',
        xValueFormatString: '####.####',
        dataPoints: this.dataPointsPeakList
      },
      {
        type: "column",
        color: 'red',
        name: 'Experimental m/z',
        showInLegend: true,
        legendText: 'Experimental m/z',
        xValueFormatString: '####.####',
        dataPoints: this.dataPointsExperimentalmz
      },
      {
        type: "column",
        color: 'green',
        name: 'Theoretical m/z',
        showInLegend: true,
        legendText: 'Theoretical m/z',
        xValueFormatString: '####.####',
        dataPoints: this.dataPointsTheoreticalmz
      }]
    });
    chart.render();
  

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