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
  PeakListIntensities: any;
  PeakListMasses: any;
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
    //this.Experimentalmz.push() //(data.InsilicoSpectra.ListExperimental_mz[i]);
    //this.Experimentalmz = tempExperimentalmz;
    //this.Theoreticalmz.push(data.InsilicoSpectra.ListTheoretical_mz[i]);
    //this.Theoreticalmz = tempTheoreticalmz;
    for (let i = 0; i < data.InsilicoSpectra.ListIndices.length; i++) {
      this.Experimentalmz.push(data.InsilicoSpectra.ListExperimental_mz[i]);
      this.Theoreticalmz.push(data.InsilicoSpectra.ListTheoretical_mz[i]);
      //this.LabelsArray.push();
    }


    this.dataSource = new MatTableDataSource(users);
    this.base64data = data.blob;
    this.ImageFilePath = this.sanitizer.bypassSecurityTrustUrl('data:image/jpg;base64,' + this.base64data);

    //this.array = data.PeakListMasses.split(",",100);
    //Data Preparation for Graph
    this.PeakListMasses = data.PeakListData.PeakListMasses.split(',').map(Number);
    this.PeakListIntensities = data.PeakListData.PeakListIntensities.split(',').map(Number);
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
    let v = Math.min(this.PeakListMasses);
//     let minDataValue = Math.min(Math.min(this.PeakListMasses), options.ticks.suggestedMin);
// let maxDataValue = Math.max(Math.max(this.PeakListMasses), options.ticks.suggestedMax);

    this.chart = new Chart('canvas', {
      type: 'bar',
      data: {
        labels: this.LabelsArray,
        datasets: [{
          label: "Experimental m/z",
          data: this.Experimentalmz,
          borderColor: 'rgba(10, 0, 10, 0.1)',
          fill: true,
          barPercentage: 0.5,
          barThickness: 6,
          maxBarThickness: 8,
          minBarLength: 2


        },
        {
          label: "Peak List Masses",
          data: this.PeakListMasses,
          borderColor: 'rgba(0, 0, 0, 0.1)',
          fill: true,
          barPercentage: 0.5,
          barThickness: 6,
          maxBarThickness: 8,
          minBarLength: 2
        },
        {
          label : "Theoretical m/z",
          data: this.Theoreticalmz,
          borderColor: 'rgba(01, 10, 0, 0.1)',
          fill: true,
          barPercentage: 0.5,
          barThickness: 6,
          maxBarThickness: 8,
          minBarLength: 2
        }

        ]
      },
      options: {
        legend:{
          display:true
        },
        // scales:{
        //   xAxes:[{display:true}],
        //   yAxes:[{display:true}]
        // },
        responsive: true,
        // title: { text: "THICCNESS SCALE", display: true },
        scales: {
          yAxes: [
            {
              ticks: {
                max: 1.5,
                min: 0,
                stepSize: 0.5
              },

              gridLines: {
                display: false,
              },
            },
          ],
          xAxes: [
            {
              gridLines: {
                display: true,
              },
              label: ["mz"]
            },
          ],
        },
        pan: {
          enabled: true,
          mode: "xy",
          speed: 1,
          threshold: 1,
        },
        zoom: {
          enabled: true,
          drag: true,
          mode: "xy",
          limits: {
            max: 1,
            min: 0.5,
          },

          ticks: {
            suggestedMin: Math.min(this.PeakListMasses),
            suggestedMax: Math.max(this.PeakListMasses)
          },
          rangeMin: {
            x: 2,
            y: 1,
          },
          rangeMax: {
            x: 10,
            y: 150,
          },
        },
      },
      plugins: {
        zoom: {
          // Container for pan options
          pan: {
            // Boolean to enable panning
            enabled: true,
      
            // Panning directions. Remove the appropriate direction to disable
            // Eg. 'y' would only allow panning in the y direction
            // A function that is called as the user is panning and returns the
            // available directions can also be used:
            //   mode: function({ chart }) {
            //     return 'xy';
            //   },
            mode: 'xy',
      
            rangeMin: {
              // Format of min pan range depends on scale type
              x: null,
              y: null
            },
            rangeMax: {
              // Format of max pan range depends on scale type
              x: null,
              y: null
            },
      
            // On category scale, factor of pan velocity
            speed: 20,
      
            // Minimal pan distance required before actually applying pan
            threshold: 10,
      
            // Function called while the user is panning
            onPan: function({chart}) { console.log(`I'm panning!!!`); },
            // Function called once panning is completed
            onPanComplete: function({chart}) { console.log(`I was panned!!!`); }
          },
      
          // Container for zoom options
          zoom: {
            // Boolean to enable zooming
            enabled: true,
      
            // Enable drag-to-zoom behavior
            drag: true,
      
            // Drag-to-zoom effect can be customized
            // drag: {
            // 	 borderColor: 'rgba(225,225,225,0.3)'
            // 	 borderWidth: 5,
            // 	 backgroundColor: 'rgb(225,225,225)',
            // 	 animationDuration: 0
            // },
      
            // Zooming directions. Remove the appropriate direction to disable
            // Eg. 'y' would only allow zooming in the y direction
            // A function that is called as the user is zooming and returns the
            // available directions can also be used:
            //   mode: function({ chart }) {
            //     return 'xy';
            //   },
            mode: 'xy',
      
            rangeMin: {
              // Format of min zoom range depends on scale type
              x: null,
              y: null
            },
            rangeMax: {
              // Format of max zoom range depends on scale type
              x: null,
              y: null
            },
      
            // Speed of zoom via mouse wheel
            // (percentage of zoom on a wheel event)
            speed: 0.1,
      
            // Minimal zoom distance required before actually applying zoom
            threshold: 2,
      
            // On category scale, minimal zoom level before actually applying zoom
            sensitivity: 3,
      
            // Function called while the user is zooming
            onZoom: function({chart}) { console.log(`I'm zooming!!!`); },
            // Function called once zooming is completed
            onZoomComplete: function({chart}) { console.log(`I was zoomed!!!`); }
          }
        }
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