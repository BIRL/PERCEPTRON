import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';
import * as firebase from 'firebase/app';

import { AngularFireAuth } from 'angularfire2/auth';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css'],
  providers: [ConfigService]
})
export class HistoryComponent implements OnInit {
  displayedColumns = ['serial', 'title', 'time','progress', 'qid'];
  dataSource: MatTableDataSource<UserData>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private route: ActivatedRoute, private router: Router, private _httpService: ConfigService, public af: AngularFireAuth) {
    const users: UserData[] = [];
    this.dataSource = new MatTableDataSource(users);
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    let top = document.getElementById('top');
    if (top !== null) {
      top.scrollIntoView();
      top = null;
    }
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  ngOnInit() {
    var user = firebase.auth().currentUser;
    if (user.emailVerified==true){
      this._httpService.getUserHistory(user).subscribe(data => this.what(data));
    }
    else{ //For Guest's Search Results & History
      this._httpService.getUserHistory(user).subscribe(data => this.what(data));
    }
    
  }

  what(data: any) {
    const users: UserData[] = [];
    for (let i = 1; i <= data.length; i++) { users.push(createNewUser(i, data[i - 1])); }
    this.dataSource = new MatTableDataSource(users);
  }


  getRecord(row) {
    var user = firebase.auth().currentUser;
    if (user.emailVerified==true){
      this._httpService.getUserHistory(user).subscribe(data => this.what(data));
    }
    else{ //For Guest's Search Results & History
      this._httpService.getUserHistory(user).subscribe(data => this.what(data));
    }
    
    if (row.progress == "Completed"){
      let x = this.router;
      x.navigate(["scans", row.qid]);
    }
    else if(row.progress == "In Queue" || row.progress == "Running"){
      alert("Dear User,\nYour query is being processed. Please wait, the search may take several minutes to complete. We appreciate your patience!\n\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team");
    }
    else if(row.progress == "Result Expired"){
      alert("Dear User,\nThe link you followed has expired. Please go to 'Protein Search Query' to submit proteoform search query.\n\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team");
    }
    else if(row.progress == "Error in Query")  // It means query wasn't able to complete properly, and there would be an issue into query parameters, Peaklist hadn't reasonable amount of data etc.
    //Therefore, it will not navigate to Scan Results.
    {
      if (window.confirm("Dear User,\nWe're sorry, your search query could not be processed because of the reasons indicated below:\nOne or more input parameters are invalid\nMissing information in the data file added\nPlease address these issues to proceed.\n\nIf problem persists, please report your problem here or contact us.\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team"))
      {
        window.location.href='https://github.com/BIRL/PERCEPTRON/issues';
      };
    }
  }
}

/** Builds and returns a new User. */
function createNewUser(id: number, data: any): UserData {
  return {
    serial: id.toString(),
    title: data.title,
    time: data.time,
    qid: data.qid,
    progress: data.progress
  };
}

export interface UserData {
  serial: string;
  title: string;
  time: string;
  qid: string;
  progress:string;
}