import { Component, OnInit, ViewChild } from '@angular/core';
import { ConfigService } from '../config.service';
import { FormControl } from '@angular/forms';

import { AngularFireAuth } from 'angularfire2/auth';
import * as firebase from 'firebase/app';
import { Router } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/map';

@Component({
  selector: 'app-fdr',
  templateUrl: './fdr.component.html',
  styleUrls: ['./fdr.component.css'],
  providers: [ConfigService]
})
export class FdrComponent implements OnInit {

  @ViewChild("imgFileInput") imgFileInput;
  TextHere: any = '';
  // filename_Model: any;
  filenameModel: any;

  stateCtrl: FormControl;
  filteredStates: Observable<any[]>;

  constructor(public af: AngularFireAuth, private router: Router, private _httpService: ConfigService, public dialog: MatDialog) {
    this.af.authState.subscribe(user => {  })

    this.stateCtrl = new FormControl();
    // this.filteredStates = this.stateCtrl.valueChanges
    //   .startWith(null)
    //   .map(state => state ? this.filterStates(state) : this.states.slice());
  }


  ngOnInit() {
  }

  onSubmit(form: any): void {
    let fi = this.imgFileInput.nativeElement;
    let stats: any = 'false';
    console.log(form);
    stats = this._httpService.postJSON(form, fi.files);
    console.log(stats)
  }

  onReset(form: any): void {
    console.log("Form has been reset");
  }

}
