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
    if (row.progress == "Completed"){
      let x = this.router;
      x.navigate(["scans", row.qid]);
    }
    else if(row.progress == "In Queue" || row.progress == "Running"){
      alert("Dear User,\n\nPlease wait while we process your query.\n\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team");
    }
    else if(row.progress == "Error in Query")  // It means query wasn't able to complete properly, and there would be an issue into query parameters, Peaklist hadn't reasonable amount of data etc.
    //Therefore, it will not navigate to Scan Results.
    {
      alert("Dear User,\n\nYour search query could not be completed due to the below possible reasons\nQuery parameters are invalid\n Peaklist has not reasonable amount of peak list information.\n Please address the above issues and if problem still persists then, report a bug using 'Report Bug' tab.\n \n\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team");
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