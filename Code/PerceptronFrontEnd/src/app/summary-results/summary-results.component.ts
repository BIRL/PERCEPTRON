import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';

@Component({
  selector: 'app-summary-results',
  templateUrl: './summary-results.component.html',
  styleUrls: ['./summary-results.component.css'],
  providers: [ConfigService]
})
export class SummaryResultsComponent implements OnInit {
  //displayedColumns = ['rank', 'name', 'id', 'molW', 'term', 'mods', 'score', 'resId'];
  displayedColumns = ['rank', 'id', 'molW', 'term', 'mods', 'score', 'resId'];
  
  dataSource: MatTableDataSource<UserData>;
  querryId: any;
  fileID: any;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private route: ActivatedRoute, private router: Router, private _httpService: ConfigService) {
    const users: UserData[] = [];
    this.dataSource = new MatTableDataSource(users);
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
        // Scrolls to top of Page after page view initialized
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
    this.route.params.subscribe((params: Params) => this.querryId = params['querryId']);
    this.route.params.subscribe((params: Params) => this.fileID = params['fileID']);
    this._httpService.GetSummaryReslts(this.querryId, this.fileID).subscribe(data => this.what(data));
  }

  what(data: any) {
    const users: UserData[] = [];
    for (let i = 1; i <= data.length; i++) { users.push(createNewUser(i, data[i - 1])); }
    //users.sort(x=>x.rank)
    this.dataSource = new MatTableDataSource(users);
  }

  getRecord(row) {
    let x = this.router;
    x.navigate(["detailedresults", this.querryId, row.resId, row.rank]);
  }
}

/** Builds and returns a new User. */
function createNewUser(id: number, data): UserData {
  return {
    rank: data.ProteinRank,
    name: data.ProteinName,
    id: data.ProteinId,
    molW: data.MolW,
    term: data.TerminalMods,
    mods: data.Mods,
    score: data.Score,
    resId: data.ResultId
  };
}

export interface UserData {
  resId: string;
  rank: string;
  name: string;
  id: string;
  molW: string;
  term: string;
  mods: string
  score: string;
}