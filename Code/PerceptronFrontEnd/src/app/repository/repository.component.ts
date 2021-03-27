import { Component, OnInit, ViewChild } from '@angular/core';
import { MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule, MatCheckbox } from '@angular/material';
import { FormGroup, FormBuilder } from '@angular/forms'
import { Http } from '@angular/http';
import { ConfigService } from '../config.service';
import { Headers } from '@angular/http';
import { FormControl } from '@angular/forms';

import { AngularFireAuth } from 'angularfire2/auth';
import * as firebase from 'firebase/app';
import { Router } from '@angular/router';

@Component({
  selector: 'app-repository',
  templateUrl: './repository.component.html',
  styleUrls: ['./repository.component.css'],
  providers: [ConfigService]
})
export class RepositoryComponent implements OnInit {
  onReset:any;
  constructor( private _httpService: ConfigService) { }
  tables1 = [//During build "DataFiles" will in "\PerceptronFrontEnd\src\assets\DataFiles" but in wwwroot all files will be in "\wwwroot\assets\DataFiles" so, give this given below path for downloading files e.g.  http://perceptron.lums.edu.pk/assets/DataFiles/HelaDataset/HELA_pk13_sw1_66sc_mono.txt
    { dataset: 'HELA_pk13_sw1_66sc_mono.txt', id: 1, download: 'assets/DataFiles/HelaDataset/HELA_pk13_sw1_66sc_mono.txt'},  //"src/assets" used 
    { dataset: 'HELA_pk17_sw1_140sc_mono.txt', id: 2, download: 'assets/DataFiles/HelaDataset/HELA_pk17_sw1_140sc_mono.txt' },
    { dataset: 'HELA_pk18_sw1_160sc_mono.txt', id: 3, download: 'assets/DataFiles/HelaDataset/HELA_pk18_sw1_160sc_mono.txt' },
    { dataset: 'HELA_pk18_sw2_160sc_mono.txt', id: 4, download: 'assets/DataFiles/HelaDataset/HELA_pk18_sw2_160sc_mono.txt'},
    { dataset: 'HELA_pk19_sw1_210sc_mono.txt', id: 5, download: 'assets/DataFiles/HelaDataset/HELA_pk19_sw1_210sc_mono.txt' },
    { dataset: 'HELA_pk19_sw2_255sc_mono.txt', id: 6, download: 'assets/DataFiles/HelaDataset/HELA_pk19_sw2_255sc_mono.txt' },
    { dataset: 'HELA_pk19_sw3.1_147sc_mono.txt', id: 7, download: 'assets/DataFiles/HelaDataset/HELA_pk19_sw3.1_147sc_mono.txt' },
    { dataset: 'HELA_pk20_sw1_205sc_mono.txt', id: 8, download: 'assets/DataFiles/HelaDataset/HELA_pk20_sw1_205sc_mono.txt' },
    { dataset: 'HELA_pk20_sw2_202sc_mono.txt', id: 9, download: 'assets/DataFiles/HelaDataset/HELA_pk20_sw2_202sc_mono.txt' },
    { dataset: 'HELA_pk20_sw3_215sc_mono.txt', id: 10, download: 'assets/DataFiles/HelaDataset/HELA_pk20_sw3_215sc_mono.txt' }
  
  ];

  
  tables2 = [
    //During build "DataFiles" will in "\PerceptronFrontEnd\src\assets\DataFiles" but in wwwroot all files will be in "\wwwroot\assets\DataFiles" so, give this given below path for downloading files e.g.  http://perceptron.lums.edu.pk/assets/DataFiles/HelaDataset/HELA_pk13_sw1_66sc_mono.txt
    { dataset: 'EcoliDataset_CID.zip', id: 1, download: 'assets/DataFiles/EcoliDataset/EcoliDataset_CID.zip'},  //"src/assets" used 
    { dataset: 'EcoliDataset_ETD.zip', id: 2, download: 'assets/DataFiles/EcoliDataset/EcoliDataset_ETD.zip' }
  ];

  tables3 = [
    //During build "DataFiles" will in "\PerceptronFrontEnd\src\assets\DataFiles" but in wwwroot all files will be in "\wwwroot\assets\DataFiles" so, give this given below path for downloading files e.g.  http://perceptron.lums.edu.pk/assets/DataFiles/HelaDataset/HELA_pk13_sw1_66sc_mono.txt
    { dataset: 'DT4_161116_mgf.zip', id: 1, download: 'assets/DataFiles/ExampleFileDataset/DT4_161116_mgf.zip'},  //"src/assets" used 
    { dataset: 'DT4_161116_mzXML.zip', id: 2, download: 'assets/DataFiles/ExampleFileDataset/DT4_161116_mzXML.zip' },
    { dataset: 'DT4_161116_mzML.zip', id: 3, download: 'assets/DataFiles/ExampleFileDataset/DT4_161116_mzML.zip' }
  ];

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
  onSubmit(form: any): void {
    var email;
    var user = firebase.auth().currentUser;
    if (user.email != null) {
      email = user.email;
      form.UserId = email;
    }
    if(form.file1==true){
      form.file1=1;
    }else{
      form.file1=0;
    }
    if(form.file2==true){
      form.file2=1;
    }else{
      form.file2=0;
    }
    if(form.file3==true){
      form.file3=1;
    }else{
      form.file3=0;
    }
    if(form.file4==true){
      form.file4=1;
    }else{
      form.file4=0;
    }
    if(form.file5==true){
      form.file5=1;
    }else{
      form.file5=0;
    }
    
    var r=this._httpService.getfile(form);
    alert("Request recorded! Kindly check your email");
}
}