import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';
@Component({
  selector: 'app-spectralcount',
  templateUrl: './spectralcount.component.html',
  styleUrls: ['./spectralcount.component.css'],
  providers: [ConfigService]
})
export class SpectralcountComponent implements OnInit {

  displayedColumns = ['serial', 'name', 'id', 'score', 'fileId'];
  dataSource: MatTableDataSource<UserData>;
  querryId: any;
  resultId: any;
  rank: any;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private route: ActivatedRoute, private router: Router, private _httpService: ConfigService) {
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
    this.route.params.subscribe((params: Params) => this.querryId = params['querryId']);
    this._httpService.GetScReslts(this.querryId).subscribe(data => this.what(data));
    // this.route.params.subscribe((params: Params) => this.resultId = params['resultId']);
    // this.route.params.subscribe((params: Params) => this.rank = params['rank']);
    // this._httpService.GetDetailedReslts(this.resultId).subscribe(data1 => this.what1(data1));
  }

  what(data: any) {
    const users: UserData[] = [];
    for (let i = 1; i <= data.length; i++) { users.push(createNewUser(i, data[i - 1])); }
    this.dataSource = new MatTableDataSource(users);

    let sequence = <HTMLLabelElement>document.getElementById("sequence");
    
    let sequenceText = data[0].Sequence;
    let text = sequenceText[0];
    
    for (let i = 1; i < sequenceText.length-1; i++) {
      if (i % 10 == 0) {
        text = text + "\xa0\xa0\xa0\xa0\xa0" + sequenceText[i];
      }
      else {
        text = text + " " + sequenceText[i];
      }
    }
    sequence.innerHTML = text;
  }

  
 
  

  getRecord(row) {
    let x = this.router;
    x.navigate(["xic", this.querryId]);
  }
}

/** Builds and returns a new User. */
function createNewUser(id: number, data: any): UserData {
  return {
    serial: id.toString(),
    name: data.ProteinName,
    id: data.Header,
    score: data.Score,
    fileId: data.FileId
    
  };
}

export interface UserData {
  serial: string;
  name: string;
  id: string;
  score: string;
  fileId: string;
  
  
}