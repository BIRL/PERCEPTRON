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

  @ViewChild("imgTargetFile") imgTargetFile;
  @ViewChild("imgDecoyFile") imgDecoyFile;

  TextHere: any = '';
  // filename_Model: any;
  NameOfTargetFile: any;
  NameOfDecoyFile : any;
  IsWaitSubmit: any;
  IsWaitDownload: any;
  UploadedFile: any;
  Uploaded_File: any;

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
    let a = 1;
  }
  ngAfterViewInit() { //Added //Updated 20201215 
    // Scrolls to top of Page after page view initialized
    let top = document.getElementById('top');
    if (top !== null) {
      top.scrollIntoView();
      top = null;
    }
  }
  
  keyPress1(event: any) {
    const pattern = /[0-9\.]/; ///     \ \a-z\@\A-Z

    let inputChar = String.fromCharCode(event.charCode);
    if (event.keyCode != 8 && !pattern.test(inputChar)) {
      confirm("Press submit button to confirm your submission");
      event.preventDefault();
    }
  }

  onSubmit(form: any): void {
    let TargetFile = this.imgTargetFile.nativeElement;
    let DecoyFile = this.imgDecoyFile.nativeElement;

    let stats: any = 'false';
    console.log(form);
    stats = this._httpService.fdrform(form, TargetFile.files);
    console.log(stats)
  }

  onReset(form: any): void {
    console.log("Form has been reset");
  }

  upload(UploadedFile:any){
    let b = 2;

  }



}
