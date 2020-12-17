import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AngularFireAuth } from 'angularfire2/auth';
import { FormControl } from '@angular/forms';
import { FormGroup, FormBuilder } from '@angular/forms'
import { Http } from '@angular/http';
import { ViewChild } from '@angular/core';
import { MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule, MatCheckbox } from '@angular/material';
import { ConfigService } from '../config.service';
import { Headers } from '@angular/http';

import * as firebase from 'firebase/app';

import { Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/map';
import { DemoComponent } from '../demo/demo.component';
@Component({
  selector: 'app-bug',
  templateUrl: './bug.component.html',
  styleUrls: ['./bug.component.css'],
  providers: [ConfigService]
})
export class BugComponent implements OnInit {

  problems = [
    {value: 'Graphical'},
    {value: 'Logical'},
    {value: 'Other'}
  ];
  selectedValue1:any;
  selectedValue2:any;

  locations = [
    {value: 'Defining the Job'},
    {value: 'Job Parameters'},
    {value: 'Job Submission'},
    {value: 'Results'},
    {value: 'Account'},
    {value: 'Other'}

  ];


  constructor(public af: AngularFireAuth, private _httpService: ConfigService, private router: Router) {

    const ctrl = new FormControl({value: 'n/a', disabled: true});
    console.log(ctrl.value);     // 'n/a'
    console.log(ctrl.status);
   }

  ngOnInit() {
  }

  ngAfterViewInit() { //Added //Updated 20201215 
    // Scrolls to top of Page after page view initialized
    let top = document.getElementById('top');
    if (top !== null) {
      top.scrollIntoView();
      top = null;
    }
  }

  
  onSubmit(form: any): void {
    var user = firebase.auth().currentUser;
    var email;
    if (user.email != null) {
      email = user.email;
      form.UserId = email;
    }

    let stats: any = 'false';
    console.log(form);
    stats = this._httpService.postbugform(form);
    console.log(stats)
    alert("Thank you for your response!");
    console.log('you submitted value:', JSON.stringify(form));
  
  }
}
