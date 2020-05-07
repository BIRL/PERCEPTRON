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
    
    let x = this.router;
    x.navigate(["scans", row.qid]);
  }
}

/** Builds and returns a new User. */
function createNewUser(id: number, data: any): UserData {
  return {
    serial: id.toString(),
    title: data.title,
    time: data.time,
    qid: data.qid,
    progress: "Completed"
  };
}

export interface UserData {
  serial: string;
  title: string;
  time: string;
  qid: string;
  progress:string;
}