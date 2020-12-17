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

  displayedColumns: string[] = ['BrowserName', 'OSWindow', 'OSLinux', 'OSMac'];
  dataSource = BrowserCompatibilityDataTable;

  ngOnInit() {
    this._httpService.stuff().subscribe(data => this.getpattern(data));
  }
  getpattern(data) {
    this.user=data.user;
    this.search=data.search;
  }
  ngAfterViewInit() { //Added //Updated 20201215 
    // Scrolls to top of Page after page view initialized
    let top = document.getElementById('top');
    if (top !== null) {
      top.scrollIntoView();
      top = null;
    }
  }

}


export interface BrowserCompatibilityTable {
  BrowserName: string;
  OSWindow: string;
  OSLinux: string;
  OSMac: string;
}

const BrowserCompatibilityDataTable: BrowserCompatibilityTable[] = [
  {BrowserName: "Google Chrome", OSWindow: 'Supported', OSLinux: "Supported", OSMac: "Supported"},
  //{BrowserName: "Opera", OSWindow: 'Supported', OSLinux: "Supported", OSMac: "---"},
  {BrowserName: "Firefox", OSWindow: 'Supported', OSLinux: "Supported", OSMac: "Supported"},
  {BrowserName: "Edge", OSWindow: 'Supported', OSLinux: "Not Available", OSMac: "Supported"},
  {BrowserName: "Safari", OSWindow: 'Not Available', OSLinux: "Not Available", OSMac: "Supported"},
];
