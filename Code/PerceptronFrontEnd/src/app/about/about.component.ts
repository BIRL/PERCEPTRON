import { Component, OnInit, ViewChild } from '@angular/core';
import { MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule } from '@angular/material';
import { AngularFireAuth } from 'angularfire2/auth';
import { Router } from '@angular/router';
import * as firebase from 'firebase/app';
import { Alert } from 'selenium-webdriver';

import {  ElementRef, Input } from '@angular/core';

import { FormGroup, FormBuilder } from '@angular/forms'
import { Http } from '@angular/http';
import {MatDialogModule} from '@angular/material';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import {  ActivatedRoute, Params } from '@angular/router';


import {FormControl} from '@angular/forms';



import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from '../config.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css'],
  providers: [ConfigService]
})
export class AboutComponent implements OnInit {
 
  constructor(private route: ActivatedRoute, private router: Router, private _httpService: ConfigService, private http: Http) { }
  user :any;
  search :any;

  ngOnInit() {
    this._httpService.stuff().subscribe(data => this.getpattern(data));
  }
  getpattern(data) {
    this.user=data.user;
    this.search=data.search;
  }
}
<<<<<<< HEAD
<<<<<<< HEAD


export interface BrowserCompatibilityTable {
  BrowserName: string;
  OSWindow: string;
  OSLinux: string;
  OSMac: string;
}

const BrowserCompatibilityDataTable: BrowserCompatibilityTable[] = [
  {BrowserName: "Google Chrome", OSWindow: 'Supported', OSLinux: "Supported", OSMac: "Supported"},
  {BrowserName: "Opera", OSWindow: 'Supported', OSLinux: "Supported", OSMac: "Supported"},
  {BrowserName: "Firefox", OSWindow: 'Supported', OSLinux: "Supported", OSMac: "Supported"},
  {BrowserName: "Edge", OSWindow: 'Supported', OSLinux: "Tested", OSMac: "Supported"},
  {BrowserName: "Safari", OSWindow: 'N/A', OSLinux: "Tested", OSMac: "Tested"},
];
=======
>>>>>>> parent of f5af0c3c... Update about.component.ts
=======
>>>>>>> parent of f5af0c3c... Update about.component.ts
