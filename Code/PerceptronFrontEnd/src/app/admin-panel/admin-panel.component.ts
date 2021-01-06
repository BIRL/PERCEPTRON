import { Component, OnInit, ViewChild } from '@angular/core';
import { ConfigService } from '../config.service';
import { FormControl } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import * as fileSaver from 'file-saver';
import { formArrayNameProvider } from '@angular/forms/src/directives/reactive_directives/form_group_name';
import { from } from 'rxjs/observable/from';


@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
  providers: [ConfigService]
})
export class AdminPanelComponent implements OnInit {

  NameOfDatabaseToBeDownloaded: any;
  filenameModel: boolean = false;
  NameOfDatabaseToBeUpdated: string;
  UploadedFile: any;
  blob : any;
  NameOfDatabase: any;
  buttonType: any;

  IsWaitSubmit = 0;
  IsWaitDownload = 0;
  stateCtrl: FormControl;
  filteredStates: Observable<any[]>;
  @ViewChild("imgFileInput") imgFileInput;

  constructor(private route: ActivatedRoute, private router: Router, private _httpService: ConfigService) {
    this.stateCtrl = new FormControl();
    this.filteredStates = this.stateCtrl.valueChanges
      .startWith(null)
      .map(state => state ? this.filterStates(state) : this.states.slice());
  }

  filterStates(name: string) {
    return this.states.filter(state =>
      state.name.toLowerCase().indexOf(name.toLowerCase()) === 0);
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

  ListOfDatabasesForUpdate = [
    { value: 'Human', viewValue: 'Human' },
    { value: 'Ecoli', viewValue: 'Ecoli' }
  ];

  ListOfDatabasesForDownload = [
    { value: 'Human', viewValue: 'Human' },
    { value: 'Ecoli', viewValue: 'Ecoli' }
  ];
  states = [
    { name: 'Human', viewValue: 'Human' },
    { name: 'Ecoli', viewValue: 'Ecoli' }
  ];

  onSubmit(buttonType, form): void {
    if(buttonType==="Upload") {
        var a = 1;
        let fi = this.imgFileInput.nativeElement;
      let stats: any = 'false';
      console.log(form);
      stats = this._httpService.postDatabase(form, fi.files);
      if (stats == 'success'){
        alert("Database successfully updated.");
      }
      console.log(stats)
    }
    if(buttonType==="Download"){
      this.IsWaitDownload = 1;
      this.NameOfDatabaseToBeDownloaded = form.ProteinDatabase;
      // this.route.params.subscribe((params: Params) => this.NameOfDatabaseToBeDownloaded = params['NameOfDatabaseToBeDownloaded']);
    this._httpService.GetDatabaseDownload(this.NameOfDatabaseToBeDownloaded).subscribe(ResultsData => this.whatResults(ResultsData));

      
      alert("Dear! User your database is successfully downloaded.");
      this.IsWaitDownload = 0;
    }
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


  upload(form: any): void {
    let test = 1;
  }
  // let fi = this.imgFileInput.nativeElement;
  //   let stats: any = 'false';
  //   console.log(form);
  //   stats = this._httpService.postJSON(form, fi.files);

}
