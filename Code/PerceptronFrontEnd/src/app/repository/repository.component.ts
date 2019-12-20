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
  tables1 = [
    { dataset: '2013_11_20_pellet_NC_07_04_MSMS_msdeconv_129.txt', id: 1, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_20_pellet_NC_07_04_MSMS_msdeconv_129.txt' },
    { dataset: '2013_11_20_pellet_NC_08_88_MSMS_msdeconv_127.txt', id: 2, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_20_pellet_NC_08_88_MSMS_msdeconv_127.txt' },
    { dataset: '2013_11_20_pellet_PD_10_83_MSMS_131121073654_msdeconv_133.txt', id: 3, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_20_pellet_PD_10_83_MSMS_131121073654_msdeconv_133.txt' },
    { dataset: '2013_11_20_pellet_PD_10_83_MSMS_131121073654_msdeconv_143.txt', id: 4, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_20_pellet_PD_10_83_MSMS_131121073654_msdeconv_143.txt'},
    { dataset: '2013_11_21_pellet_NC_05_10_MSMS_msdeconv_140.txt', id: 5, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_21_pellet_NC_05_10_MSMS_msdeconv_140.txt' },
    { dataset: '2013_11_21_pellet_NC_05_10_MSMS_msdeconv_147.txt', id: 6, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_21_pellet_NC_05_10_MSMS_msdeconv_147.txt' },
    { dataset: '2013_11_21_pellet_NC_09_57_MSMS_msdeconv_128.txt', id: 7, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_21_pellet_NC_09_57_MSMS_msdeconv_128.txt' },
    { dataset: '2013_11_21_pellet_NC_09_57_MSMS_msdeconv_166.txt', id: 8, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_21_pellet_NC_09_57_MSMS_msdeconv_166.txt' },
    { dataset: '2013_11_21_pellet_PD_00_27_MSMS_msdeconv_129.txt', id: 9, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_21_pellet_PD_00_27_MSMS_msdeconv_129.txt' },
    { dataset: '2013_11_21_pellet_PDAD_02_10_MSMS_msdeconv_141.txt', id: 10, download: 'http://perceptron.lums.edu.pk/assets/images/data/2013_11_21_pellet_PDAD_02_10_MSMS_msdeconv_141.txt' },
  
  ];

  tables2 = [
    { dataset: 'HELA_pk17_sw1_66sc_mono.txt', id: 1, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk13_sw1_66sc_mono.txt' },
    { dataset: 'HELA_pk17_sw1_140sc_mono.txt', id: 2, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk17_sw1_140sc_mono.txt' },
    { dataset: 'HELA_pk18_sw1_160sc_mono.txt', id: 3, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk18_sw1_160sc_mono.txt' },
    { dataset: 'HELA_pk19_sw2_255sc_mono.txt', id: 4, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk19_sw2_255sc_mono.txt'},
    { dataset: 'HELA_pk19_sw3.1_147sc_mono.txt', id: 5, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk19_sw3.1_147sc_mono.txt' },
    { dataset: 'HELA_pk18_sw2_160sc_mono.txt', id: 6, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk18_sw2_160sc_mono.txt' },
    { dataset: 'HELA_pk20_sw1_205sc_mono.txt', id: 7, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk20_sw1_205sc_mono.txt' },
    { dataset: 'HELA_pk20_sw2_202sc_mono.txt', id: 8, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk20_sw2_202sc_mono.txt' },
    { dataset: 'HELA_pk20_sw3_215sc_mono.txt', id: 9, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk20_sw3_215sc_mono.txt' },
    { dataset: 'HELA_pk19_sw1_210sc_mono.txt', id: 10, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk19_sw1_210sc_mono.txt' },
  
  ];
  tables3 = [
    { dataset: 'CSM021_Roma_HCD_05102015_msdeconv_1130.txt', id: 1, download: 'http://perceptron.lums.edu.pk/assets/images/data/CSM021_Roma_HCD_05102015_msdeconv_1130.txt' },
    { dataset: 'CSM021_Roma_HCD_05102015_msdeconv_1131.txt', id: 2, download: 'http://perceptron.lums.edu.pk/assets/images/data/CSM021_Roma_HCD_05102015_msdeconv_1131.txt' },
    { dataset: 'CSM021_Roma_HCD_05102015_msdeconv_1132.txt', id: 3, download: 'http://perceptron.lums.edu.pk/assets/images/data/CSM021_Roma_HCD_05102015_msdeconv_1132.txt' },
    { dataset: 'CSM021_Roma_HCD_05102015_msdeconv_1993.txt', id: 4, download: 'http://perceptron.lums.edu.pk/assets/images/data/CSM021_Roma_HCD_05102015_msdeconv_1993.txt'},
    { dataset: 'CSM021_Roma_HCD_05102015_msdeconv_1995.txt', id: 5, download: 'http://perceptron.lums.edu.pk/assets/images/data/CSM021_Roma_HCD_05102015_msdeconv_1995.txt' },
    { dataset: 'CSM021_Roma_HCD_05102015_msdeconv_1996-.txt', id: 6, download: 'http://perceptron.lums.edu.pk/assets/images/data/CSM021_Roma_HCD_05102015_msdeconv_1996.txt' },
    { dataset: 'CSM021_Roma_HCD_05102015_msdeconv_1997.txt', id: 7, download: 'http://perceptron.lums.edu.pk/assets/images/data/CSM021_Roma_HCD_05102015_msdeconv_1997.txt' },
    { dataset: 'CSM028_Roma_HCD_07102015_msdeconv_1478.txt', id: 8, download: 'http://perceptron.lums.edu.pk/assets/images/data/CSM028_Roma_HCD_07102015_msdeconv_1478.txt' },
    { dataset: 'SM015_Roma_HCD_100915_msdeconv_1419.txt', id: 9, download: 'http://perceptron.lums.edu.pk/assets/images/data/SM015_Roma_HCD_100915_msdeconv_1419.txt' },
    { dataset: 'SM015_Roma_HCD_100915_msdeconv_1420.txt', id: 10, download: 'http://perceptron.lums.edu.pk/assets/images/data/SM015_Roma_HCD_100915_msdeconv_1420.txt' },
  
  ];
  tables4 = [
    { dataset: 'HELA_pk17_sw1_66sc_mono.txt', id: 1, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk13_sw1_66sc_mono.txt' },
    { dataset: 'HELA_pk17_sw1_140sc_mono.txt', id: 2, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk17_sw1_140sc_mono.txt' },
    { dataset: 'HELA_pk18_sw1_160sc_mono.txt', id: 3, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk18_sw1_160sc_mono.txt' },
    { dataset: 'HELA_pk19_sw2_255sc_mono.txt', id: 4, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk19_sw2_255sc_mono.txt'},
    { dataset: 'HELA_pk19_sw3.1_147sc_mono.txt', id: 5, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk19_sw3.1_147sc_mono.txt' },
    { dataset: 'HELA_pk18_sw2_160sc_mono.txt', id: 6, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk18_sw2_160sc_mono.txt' },
    { dataset: 'HELA_pk20_sw1_205sc_mono.txt', id: 7, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk20_sw1_205sc_mono.txt' },
    { dataset: 'HELA_pk20_sw2_202sc_mono.txt', id: 8, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk20_sw2_202sc_mono.txt' },
    { dataset: 'HELA_pk20_sw3_215sc_mono.txt', id: 9, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk20_sw3_215sc_mono.txt' },
    { dataset: 'HELA_pk19_sw1_210sc_mono.txt', id: 10, download: 'http://perceptron.lums.edu.pk/assets/images/data/HELA_pk19_sw1_210sc_mono.txt' },
  
  ];
  tables5 = [
    { dataset: 'BW_20_1_cidetd_msdecv28715.txt', description: "Text Text Text Text Text Text Text Text Text Text \n Text Text Text Text Text TextText Text Text Text", id: '1', download: 'http://perceptron.lums.edu.pk/assets/images/data/BW_20_1_111106004325_cidetd_msdeconv2815.txt' },
    { dataset: 'BW_20_2_cidetd_msdecv28197.txt', description: "Text Text Text Text Text Text Text Text Text Text \n Text Text Text Text Text TextText Text Text Text", id: '2',  download: 'http://perceptron.lums.edu.pk/assets/images/data/BW_20_1_111106004325_cidetd_msdeconv2817.txt' },
    { dataset: 'BW_20_5_cidetd_msdecv28139.txt', description: "Text Text Text Text Text Text Text Text Text Text \n Text Text Text Text Text TextText Text Text Text", id: '3',  download: 'http://perceptron.lums.edu.pk/assets/images/data/BW_20_1_111106004325_cidetd_msdeconv2819.txt' }, 
  ];
  ngOnInit() {
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