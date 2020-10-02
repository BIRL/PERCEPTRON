import { Component, OnInit } from '@angular/core';
import { ConfigService } from '../config.service';
import { FormControl } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs/Observable';


@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
  providers: [ConfigService]
})
export class AdminPanelComponent implements OnInit {

  NameOfDatabase: any;
  filenameModel: any;
  NameOfDatabaseToBeUpdated: any;
  UploadedFile: any;

  IsWaitSubmit = 0;
  IsWaitDownload = 0;
  stateCtrl: FormControl;
  filteredStates: Observable<any[]>;

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

  onSubmit(file: any): void {
    var a = 1;
  }

  onDownload(form: any): void {
    this.IsWaitDownload = 1;
    alert("Dear! User your database is successfully downloaded.");
    this.IsWaitDownload = 0;
  }


  upload(UploadedFile: any): void {

  }
  // let fi = this.imgFileInput.nativeElement;
  //   let stats: any = 'false';
  //   console.log(form);
  //   stats = this._httpService.postJSON(form, fi.files);

}
