// import { Component, OnInit, ElementRef, Input, ViewChild } from '@angular/core';
// import {MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule} from '@angular/material';
// import { FormGroup, FormBuilder } from '@angular/forms'
// import { Http } from '@angular/http';
// import {MatDialogModule} from '@angular/material';
// import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
// import { Router, ActivatedRoute, Params } from '@angular/router';
// import {MatProgressBarModule} from '@angular/material';
// import {MatSnackBar} from '@angular/material';


// import {FormControl} from '@angular/forms';
// import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
// import { AngularFireAuth } from 'angularfire2/auth';
// import * as firebase from 'firebase/app';
// import { Injectable } from '@angular/core';
// import { HttpClient } from '@angular/common/http';


// import {Observable} from 'rxjs/Observable';
// import 'rxjs/add/operator/startWith';
// import 'rxjs/add/operator/map';
// import { ConfigService } from '../config.service';


// @Component({
//   selector: 'app-patterngenerator',
//   templateUrl: './patterngenerator.component.html',
//   styleUrls: ['./patterngenerator.component.css'],
//   providers: [ConfigService]
// })
// export class PatterngeneratorComponent implements OnInit {
//   diableEmail: boolean;
//   Carbon: any;
//   color='primary';
//   mode = 'determinate';
//   value = 50;
//     displayedColumns = ['serial', 'name', 'id'];
//     dataSource: MatTableDataSource<UserData>;
//     Hydrogen: any;
//   Nitrogen: any;
//   Oxygen: any;
//   Sulphur: any;
//   quid:any;
//   constructor(private route: ActivatedRoute, private router: Router, private _httpService: ConfigService, private http: Http, public snackBar: MatSnackBar) { }
//   postData: string;
//   postData1: string = "Ahsan";
//   public lineChartData:Array<any> = [
//     {data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0], label: 'Intensity'},

//   ];
//   public lineChartLabels:Array<any> = ['', '', '', '', '', '', '', '','', ''];
//   public lineChartOptions:any = {
//     responsive: true
//   };
  


//   public lineChartColors:Array<any> = [
//     { // grey
//       backgroundColor: 'rgba(148,159,177,0.2)',
//       borderColor: 'rgba(148,159,177,1)',
//       pointBackgroundColor: 'rgba(148,159,177,1)',
//       pointBorderColor: '#fff',
//       pointHoverBackgroundColor: '#fff',
//       pointHoverBorderColor: 'rgba(148,159,177,0.8)'
//     },
//     { // dark grey
//       backgroundColor: 'rgba(77,83,96,0.2)',
//       borderColor: 'rgba(77,83,96,1)',
//       pointBackgroundColor: 'rgba(77,83,96,1)',
//       pointBorderColor: '#fff',
//       pointHoverBackgroundColor: '#fff',
//       pointHoverBorderColor: 'rgba(77,83,96,1)'
//     },
//     { // grey
//       backgroundColor: 'rgba(148,159,177,0.2)',
//       borderColor: 'rgba(148,159,177,1)',
//       pointBackgroundColor: 'rgba(148,159,177,1)',
//       pointBorderColor: '#fff',
//       pointHoverBackgroundColor: '#fff',
//       pointHoverBorderColor: 'rgba(148,159,177,0.8)'
//     }
//   ];

//   public lineChartLegend:boolean = true;
//   public lineChartType:string = 'line';
 
 
//   // events
//   // public chartClicked(e:any):void {
//   //   console.log(e);
//   // }
 
//   // public chartHovered(e:any):void {
//   //   console.log(e);
//   // }

//   ngOnInit() {
//     var user = firebase.auth().currentUser;
//     if (user.email != null) {
//       this.diableEmail = true;
//     }
//     else {
//       this.diableEmail = false;
//     }
//   }
//   keyPress(event: any) {
//     const pattern = /[0-9\ ]/;

//     let inputChar = String.fromCharCode(event.charCode);
//     if (event.keyCode != 8 && !pattern.test(inputChar)) {
//       confirm("Only integers are allowed");
//       event.preventDefault();
//     }
//   }
//   onSubmit(form: any): void {

//     if (this.Carbon == 0 && this.Hydrogen == 0 && this.Oxygen == 0 && this.Sulphur == 0 && this.Nitrogen == 0){
//       alert("All  the values cannot be null")
//     }
//     else {if(this.Carbon > 50000 || this.Hydrogen > 100000 || this.Oxygen > 25000 || this.Sulphur > 1000 || this.Nitrogen > 20000) {
//       alert("Values out of range!")
//     }
//     else{
//       this._httpService.postpattern(form).subscribe(data => this.getpattern(data));
//     }
// }   
// }

//   //  openSnackBar() {
//   //   this.snackBar.open(this.postData, "Okay", {
//   //     duration: 2000,
//   //   });
//   // }

//  getpattern(data: any) {
//    this.lineChartData = data.intensities;
//    const clone = data.indices;
//    this.lineChartLabels.length = 0;
//    for (let i = 0; i < clone.length; ++i) {
//     this.lineChartLabels.push(clone[i]);
//   }
//   this.quid = data.quid;
//   this.quid = "http://perceptron.lums.edu.pk/perceptron2/assets/images/text/"+this.quid+".txt";
//    const users: UserData[] = [];
//    for (let i = 1; i <= data.intensities.length; i++) { users.push(createNewUser(i, data.intensities[i - 1],data.indices[i - 1])); }
//    this.dataSource = new MatTableDataSource(users);
//  }

// }
// function createNewUser(id: number, data: any,data1: any): UserData {
//   return {
//     serial: id.toString(),
//     name: data1,
//     id: data,
//   };
// }

// export interface UserData {
//   serial: string;
//   name: string;
//   id: string;
// }
