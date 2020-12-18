import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';
import * as fileSaver from 'file-saver';


@Component({
  selector: 'app-scan-view',
  templateUrl: './scan-view.component.html',
  styleUrls: ['./scan-view.component.css'],
  providers: [ConfigService]
})
export class ScanViewComponent implements OnInit {
  //displayedColumns = ['serial', 'name', 'id', 'score', 'molW', 'truncation', 'frags', 'mods', 'mix', 'fileId'];
  displayedColumns = ['serial', 'name', 'id', 'score', 'molW', 'fileId'];
  users: UserData[] = [];
  dataSource: MatTableDataSource<UserData>;
  querryId: any;
  blob: any;

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
    this._httpService.GetScanReslts(this.querryId).subscribe(data => this.what(data));
    //this._httpService.downloadFile(this.querryId).subscribe(data => this.what(data));
  }

  what(data: any) {
    
    for (let i = 1; i <= data.length; i++) { this.users.push(createNewUser(i, data[i - 1])); }
    this.dataSource = new MatTableDataSource(this.users);

    if (this.users.length == 0){  //BatchMode: //Checking if "users" array is empty then, it will be considered (PERCEPTRON will not fetch the data from database) as batch mode or query error but the later one has been addressed in 'history.component.ts' by applying conditions in this function {getRecord(row)} and therefore, now just Batch Mode's condition is required here.
      alert("Dear User,\nSearch Results file is too large and will be downloaded. Please click Results Download to proceed.\n\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team");
    } 


   


    // let title = <HTMLLabelElement>document.getElementById("SearchTitle");
    // title.innerHTML = data.Paramters.SearchParameters.Title;

    // let pdb = <HTMLLabelElement>document.getElementById("ProteinDB");
    // pdb.innerHTML = data.Paramters.SearchParameters.ProteinDatabase;


    // let protTol = <HTMLLabelElement>document.getElementById("protTol");
    // protTol.innerHTML = data.Paramters.SearchParameters.MwTolerance;

    // let autotunee = <HTMLLabelElement>document.getElementById("Tuner");
    // autotunee.innerHTML = data.Paramters.SearchParameters.Autotune;

    // let ppeptol = <HTMLLabelElement>document.getElementById("peptol");
    // ppeptol.innerHTML = data.Paramters.SearchParameters.HopThreshhold;

    // let fragt = <HTMLLabelElement>document.getElementById("FragType");
    // fragt.innerHTML = data.Paramters.SearchParameters.InsilicoFragType;

    // let SpecI = <HTMLLabelElement>document.getElementById("SI");
    // SpecI.innerHTML = data.Paramters.SearchParameters.HandleIons;

    // let DenovAllow = <HTMLLabelElement>document.getElementById("PST");
    // DenovAllow.innerHTML = data.Paramters.SearchParameters.DenovoAllow;

    // let PstLength = <HTMLLabelElement>document.getElementById("PSTLen");
    // PstLength.innerHTML = data.Paramters.SearchParameters.MinimumPstLength + " " + data.Paramters.SearchParameters.MaximumPstLength;

    // let IPMSWeight = <HTMLLabelElement>document.getElementById("Slider1");
    // let PSTWeight = <HTMLLabelElement>document.getElementById("Slider2");
    // let SpecCompWeight = <HTMLLabelElement>document.getElementById("Slider3");
    

  }


  getRecord(row) {
    let x = this.router;
    x.navigate(["summaryresults", this.querryId, row.fileId]);
  }
  download(){
    this.route.params.subscribe((params: Params) => this.querryId = params['querryId']);
    this._httpService.GetResultsDownload(this.querryId).subscribe(ResultsData => this.whatResults(ResultsData));

    
    // let x = this.router;
    // x.navigate(["resultsdownload", this.querryId]);
  }

  whatResults(ResultsData: any) {

    //See it later just for #Know
    //var Data = this.sanitizer.bypassSecurityTrustUrl('data:text/plain;base64,' + filedata);

    for (let i = 0; i < ResultsData.ListOfFileBlobs.length; i++) {  //#ENNT: Can be Removed...
      let FileName = ResultsData.ZipFileWithPath;
      let CheckFileType = FileName.split('.').pop();
      let IndividualResultsFile = ResultsData.ListOfFileBlobs[i];

      const byteCharacters = atob(IndividualResultsFile);
      const byteNumbers = new Array(byteCharacters.length);
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      this.blob = new Blob([byteArray], { type: "application/zip;charset=utf-8" })//"text/plain;charset=utf-8" });

      fileSaver.saveAs(this.blob, FileName);


      console.log(FileName + "Successfully Downloaded!!!");

    }
    //alert("Dear User Your Result File(s) Downloaded. \nPlease See Your Download Folder.");


  }

  //Resutls Download Working...
    // download() {
  //   var abd = this.downloadFile(this.querryId).subscribe(response => {
	// 		let blob:any = new Blob([response.blob()], { type: 'text; charset=utf-8' });
	// 		const url= window.URL.createObjectURL(blob);
	// 		window.open(url);
	// 		window.location.href = response.url;
	// 		fileSaver.saveAs(blob, 'Results.txt');
  //   })
  //   // , error => console.log('Error downloading the file'),
  //   //              () => console.info('File downloaded successfully');
  // }
}

/** Builds and returns a new User. */
function createNewUser(id: number, data: any): UserData {
  return {
    serial: id.toString(),
    name: data.FileName,
    id: data.ProteinId,
    score: data.Score,
    molW: data.MolW,
    fileId: data.FileId,
    SearchModeMessage: data.SearchModeMessage

    // truncation: data.Truncation,
    // frags: data.Frags,
    // mods: data.Mods,
    // mix: data.Time,    
  };
}

export interface UserData {
  serial: string;
  name: string;
  id: string;
  score: string;
  molW: string;
  fileId: string;
  SearchModeMessage: string;

  // truncation: string;
  // frags: string;
  // mods: string;
  // mix: string;
}