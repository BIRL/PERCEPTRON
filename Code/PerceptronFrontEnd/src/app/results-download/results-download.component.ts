import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';
import { DomSanitizer } from '@angular/platform-browser';
import * as fileSaver from 'file-saver';

@Component({
  selector: 'app-results-download',
  templateUrl: './results-download.component.html',
  styleUrls: ['./results-download.component.css'],
  providers: [ConfigService]
})
export class ResultsDownloadComponent implements OnInit {
  querryId: any;
  blob: any;

  constructor(private route: ActivatedRoute, private sanitizer: DomSanitizer, private _httpService: ConfigService) { }

  ngOnInit() {
    this.route.params.subscribe((params: Params) => this.querryId = params['querryId']);
    this._httpService.GetResultsDownload(this.querryId).subscribe(ResultsData => this.what(ResultsData));
  }

  what(ResultsData: any) {

    //See it later just for #Know
    //var Data = this.sanitizer.bypassSecurityTrustUrl('data:text/plain;base64,' + filedata);

    for (let i = 0; i < ResultsData.ListOfFileBlobs.length; i++) {
      let FileName = ResultsData.AllResultFilesNames[i];
      let CheckFileType =  FileName.split('.').pop();
      let IndividualResultsFile = ResultsData.ListOfFileBlobs[i];

      const byteCharacters = atob(IndividualResultsFile);
      const byteNumbers = new Array(byteCharacters.length);
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      //const blob = new Blob();
      if (CheckFileType == "txt"){
        this.blob = new Blob([byteArray], { type: "text/plain;charset=utf-8" });
      }
      else if(CheckFileType == "csv"){
        this.blob = new Blob([byteArray], { type: "text/csv;charset=utf-8" });
      }
      //const blobUrl = URL.createObjectURL(blob);

      fileSaver.saveAs(this.blob, FileName);
      console.log(FileName + "Successfully Downloaded!!!");
    }
    alert("Dear User Your Result File(s) Downloaded. \nPlease See Your Download Folder.");


  }

}
