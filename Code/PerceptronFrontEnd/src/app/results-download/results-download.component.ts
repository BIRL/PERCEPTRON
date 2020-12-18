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
  ngAfterViewInit() { //Added //Updated 20201215 
    // Scrolls to top of Page after page view initialized
    let top = document.getElementById('top');
    if (top !== null) {
      top.scrollIntoView();
      top = null;
    }
  }

  what(ResultsData: any) {

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
}
