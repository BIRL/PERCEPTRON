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

import { Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/map';
import { DemoComponent } from '../demo/demo.component';
import { CloseScrollStrategy } from '@angular/cdk/overlay';
import { from } from 'rxjs/observable/from';


@Component({
  selector: 'app-protein-search',
  templateUrl: './protein-search.component.html',
  styleUrls: ['./protein-search.component.css'],
  providers: [ConfigService]
})

export class ProteinSearchComponent implements OnInit {
  @ViewChild("imgFileInput") imgFileInput;
  @ViewChild("PtmAllow") PtmAllow;

  ListOfDatabases = [
    // { value: 'Swissprot', viewValue: 'Swissprot' },
    // { value: 'TrEMBL', viewValue: 'TrEMBL' },
    { value: 'Human', viewValue: 'Human' },
    { value: 'Ecoli', viewValue: 'Ecoli' }
  ];

  YesNo = [
    { value: '1', viewValue: 'Yes' },
    { value: '0', viewValue: 'No' },
  ];


  states = [
    { name: 'Human', viewValue: 'Human' },
    { name: 'Ecoli', viewValue: 'Ecoli' }
  ]; //    { name: 'Swissprot', viewValue: 'Swissprot' }, { name: 'TrEMBL', viewValue: 'TrEMBL' },

  diableEmail: boolean;
  name: any;
  Neutral_Loss: any;
  Slider_Value: any;
  Units_mass4:any;
  animal: string;
  state: string = '';
  upload:any;
  Uploaded_File:any;
  
  postData: string;
  Mass_Tolerance_Unit: any;
  Hop_Tolerance_Unit: any;
  PtmToleranceUnit: any;


  EmailId: string ='';
  Title: any = '';
  MassMode: any = '';
  Mass_Tolerance: any = '';
  Autotune: any = '';
  Peptide_Tolerance: any = '';
  PeptideToleranceUnit : any = '';
  FilterDB: any = '';
  // PTM_Tolerance: any = '';
  Terminal_Modif: any = '';
  Slider1: any = '';
  Slider2: any = '';
  Slider3: any = '';
  ProtDb: any = '';
  Recieve_Top: any = '';
  Special_Ions: any = '';
  MinimumPstLength: any = '';
  MaximumPstLength: any = '';
  Methionine_ChemicalModification: any = '';
  Cysteine_ChemicalModification: any = '';
  Truncation: any = '';  //Trunc
  Units_mass: any = '';
  NameOfDatabase: any = '';
  NoOfOutputResults: any = '';
  selectedFrag: any = '';
  SelectedSpecialIons: any = '';
  Denovo_Allow: any = '';
  Terminal_Modification: any = ''; //[];
  HopThreshhold: any;
  PSTTolerance: any;
  PtmTolerance : any;
  VariableModifications: any = '';
  FixedModifications : any = '';

  InstrumentAccuracy: any;
  InstrumentAccuracy1: any;

  //////Placeholder Variables to avoid ng build --prod --aot error
  PST_Tolerance: any;
  Maximum_PstLength:any;
  Hop_Threshhold:any;
  filenameModel:any
  ////

  stateCtrl: FormControl;
  filteredStates: Observable<any[]>;


  fragments = [
    { value: 'CID', viewValue: 'CID' },
    { value: 'ECD', viewValue: 'ECD' },
    { value: 'ETD', viewValue: 'ETD' },
    { value: 'EDD', viewValue: 'EDD' },
    { value: 'BIRD', viewValue: 'BIRD' },
    { value: 'HCD', viewValue: 'HCD' },
    { value: 'SID', viewValue: 'SID' },
    { value: 'IMD', viewValue: 'IMD' },
    { value: 'NETD', viewValue: 'NETD' }
  ];

  SpecialIonz = [];
  EmptyArray = [];

  Special1 = [
    { value: 'bo', viewValue: 'b°' },
    { value: 'bstar', viewValue: 'b*' },
    { value: 'yo', viewValue: 'y°' },
    { value: 'ystar', viewValue: 'y*' }
  ];

  Special2 = [
    { value: 'zd', viewValue: 'z\'' },
    { value: 'zdd', viewValue: 'z\"' }
  ];

  Special3 = [
    { value: 'ao', viewValue: 'a°' },
    { value: 'astar', viewValue: 'a*' }
  ];

  TerminalMods = [
    { value: 'None', viewValue: 'None' },
    { value: 'NME', viewValue: 'N-Methionine Excision' },
    { value: 'NME_Acetylation', viewValue: 'N-Methionine Excision and Acetylation' },
    { value: 'M_Acetylation', viewValue: 'N-Methionine Acetylation' }
  ];

  Cys = [
    { value: 'None', viewValue: 'None' },
    { value: 'Cys_CAM', viewValue: 'Carboxyamidomethyl Cysteine' },
    { value: 'Cys_PE', viewValue: 'Pyridyl-Ethyl Cysteine' },
    { value: 'Cys_CM', viewValue: 'Carboxymethyl Cysteine' },
    { value: 'Cys_PAM', viewValue: 'Propionamide Cysteine ' }
  ];

  Meth = [
    { value: 'None', viewValue: 'None' },
    { value: 'MSO', viewValue: 'Methionine Sulfoxide' },
    { value: 'MSONE', viewValue: 'Methionine Sulfone' }
  ];

  ListOutputResults = [
    { value: '10', viewValue: '10' },
    { value: '20', viewValue: '20' },
    { value: '30', viewValue: '30' },
    { value: '40', viewValue: '40' },
    { value: '50', viewValue: '50' },
    { value: '60', viewValue: '60' },
    { value: '70', viewValue: '70' },
    { value: '80', viewValue: '80' },
    { value: '90', viewValue: '90' },
    { value: '100', viewValue: '100' }

  ];

  MinTagLength = [
    { value: '2', viewValue: '2' },
    { value: '3', viewValue: '3' },
    { value: '4', viewValue: '4' },
    { value: '5', viewValue: '5' },
    { value: '6', viewValue: '6' }
  ];

  MaxTagLength = [];

  units = [
    { value: 'Da', viewValue: 'Da' },
    { value: 'ppm', viewValue: 'ppm' },
    { value: 'mmu', viewValue: 'mmu' },
    //{ value: '%', viewValue: '%' },

  ];

  UnitOfHopThreshhold = [
    { value: 'Da', viewValue: 'Da' },
  ]; 


  constructor(public af: AngularFireAuth, private router: Router, private _httpService: ConfigService, public dialog: MatDialog) {
    this.af.authState.subscribe(user => {  })

    this.stateCtrl = new FormControl();
    this.filteredStates = this.stateCtrl.valueChanges
      .startWith(null)
      .map(state => state ? this.filterStates(state) : this.states.slice());
  }

  filterStates(name: string) {
    return this.states.filter(state =>
      state.name.toLowerCase().indexOf(name.toLowerCase()) === 0);
  }


  selectIons() {

    this.SpecialIonz = this.EmptyArray;
    if (this.selectedFrag == 'CID' || this.selectedFrag == 'IMD' || this.selectedFrag == 'BIRD' || this.selectedFrag == 'SID' || this.selectedFrag == 'HCD') {
      this.SpecialIonz = this.Special1;
    }

    if (this.selectedFrag == 'ECD' || this.selectedFrag == 'ETD') {
      this.SpecialIonz = this.Special2;
    }

    if (this.selectedFrag == 'NETD' || this.selectedFrag == 'EDD') {
      this.SpecialIonz = this.Special3;
    }
  }

  toggle() {

    var element = <HTMLInputElement>document.getElementById("MinNoiseIntensity");
    if (element.disabled == false) element.disabled = true; else element.disabled = false;

    var element = <HTMLInputElement>document.getElementById("ChiThreshold");
    if (element.disabled == false) element.disabled = true; else element.disabled = false;
    var element = <HTMLInputElement>document.getElementById("WinLenDeconv");
    if (element.disabled == false) element.disabled = true; else element.disabled = false;
    var element = <HTMLInputElement>document.getElementById("MassSkippingFactor");
    if (element.disabled == false) element.disabled = false; else element.disabled = false;
    var element = <HTMLInputElement>document.getElementById("InstrumentAccuracy");
    if (element.disabled == false) element.disabled = true; else element.disabled = false;

  }

  SetMaxTagLength() {
    this.MaxTagLength = this.EmptyArray;
    var start = parseInt(this.MinimumPstLength) + 1;
    for (var i = start; i < 9; i++) {
      this.MaxTagLength.push({ value: i, viewValue: i });
    }
  }

  keyPress(event: any) {
    const pattern = /[0-9\.\ ]/;

    let inputChar = String.fromCharCode(event.charCode);
    if (event.keyCode != 8 && !pattern.test(inputChar)) {
      confirm("Only integers are allowed");
      event.preventDefault();
    }
  }

  keyPress1(event: any) {
    const pattern = /[0-9\+\-\.\ \a-z\@\A-Z]/;

    let inputChar = String.fromCharCode(event.charCode);
    if (event.keyCode != 8 && !pattern.test(inputChar)) {
      confirm("Press submit button to confirm your submission");
      event.preventDefault();
    }
  }

  LoadDefaults() { // Here is Load Default Parameters
    this.Title = "Default Run";
    this.EmailId = '';
    this.NameOfDatabase = 'Human';
    this.NoOfOutputResults = '100';
    this.MassMode = 2;   // For Selecting M(Neutral)
    this.FilterDB = true;
    this.Mass_Tolerance = 500;
    this.Peptide_Tolerance = 15;
    this.PeptideToleranceUnit = 'ppm';
    this.Autotune = false;
    this.selectedFrag = 'HCD';
    this.SpecialIonz = this.Special1;
    this.SelectedSpecialIons = ['bo','bstar','yo','ystar'];

    this.Denovo_Allow = true;
    this.MinimumPstLength = '3';
    this.SetMaxTagLength();
    this.MaximumPstLength = this.Maximum_PstLength = 6;  // Maximum_PstLength: Used because of  [(ngModel)]
    this.HopThreshhold = this.Hop_Threshhold = 0.1;  //Tolerance for each hop  // Hop_Threshhold: Used because of  [(ngModel)]
    this.PSTTolerance = this.PST_Tolerance = 0.45;  //Overall tolerance for PST // PST_Tolerance: Used because of  [(ngModel)]

    this.Truncation = false;
    this.Terminal_Modification = ['None', 'NME', 'NME_Acetylation', 'M_Acetylation'];//(['None', 'NME', 'NME_Acetylation', 'M_Acetylation']).join(",");//"None, NME, NME_Acetylation, M_Acetylation";
    this.PtmAllow = false;
    this.PtmTolerance = 0.5;
    this.PtmToleranceUnit = 'Da';
    this.VariableModifications = "";
    this.FixedModifications = "";


    this.Slider1 = '0'; //Intact Protein Mass Slider
    this.Slider2 = '0'; //PST Slider
    this.Slider3 = '100'; //Insilico Slider


  }


  ngOnInit() {
    this.Mass_Tolerance_Unit = 'Da';
    this.Neutral_Loss = 0.0;
    this.Slider_Value = 0.0;

    this.Hop_Tolerance_Unit = 'Da';

    this.PtmToleranceUnit = 'Da';
    this.Methionine_ChemicalModification = "None";
    this.Cysteine_ChemicalModification = "None";
    var user = firebase.auth().currentUser;
    if (user.emailVerified == false) {
      this.diableEmail = false;
    }
    else {
      this.diableEmail = true;
    }
  }

  disableMods() {

    var Voptionx = (<HTMLSelectElement>document.getElementById("List_of_Modifications"));
    var Foption = (<HTMLSelectElement>document.getElementById("Fixed_Modification"));
    var Voption = (<HTMLSelectElement>document.getElementById("Variable_Modifications"));
   

    if (Foption == null) {

    }
    else {
      var selectedOpts = Foption.options;
      var len = selectedOpts.length
      for (let i = 0; i < len; i++) {
        Voptionx.append(selectedOpts[i]);
      }
    }

    if (Voption == null) {

    }
    else {
      var selectedOptss = Voption.options;
      var lenx = selectedOptss.length
      for (let i = 0; i < lenx; i++) {
        Voptionx.append(<HTMLOptionElement>selectedOptss[i]);
      }
    }
  }

  fix_add_mod() {
    var optionx = <HTMLSelectElement>document.getElementById("List_of_Modifications");
    var selectedOpts = optionx.selectedOptions;
    if (selectedOpts.length == 0) {
      alert("First, please select type of modification from List of Modifications.");
      //alert("Nothing to move - Add.");   //Just for now 20200710
    }

    var Voptionx = <HTMLSelectElement>document.getElementById("Fixed_Modification");
    var len = selectedOpts.length
    for (let i = 0; i < len; i++) {
      Voptionx.append(selectedOpts[i]);
    }
  }

  fix_remove_mod() {
    var optionx = <HTMLSelectElement>document.getElementById("Fixed_Modification");
    var selectedOpts = optionx.selectedOptions;
    if (selectedOpts.length == 0) {
      alert("If it is not already empty then, please select type of modification you want to remove from Fixed Modications.");
      // alert("Nothing to move - Add.");
    }
    var Voptionx = <HTMLSelectElement>document.getElementById("List_of_Modifications");
    var len = selectedOpts.length
    for (let i = 0; i < len; i++) {
      Voptionx.append(selectedOpts[i]);
    }
  }

  add_mod() {
    var optionx = <HTMLSelectElement>document.getElementById("List_of_Modifications");
    var selectedOpts = optionx.selectedOptions;
    if (selectedOpts.length == 0) {
      alert("First, please select type of modification from List of Modifications.");
      //alert("Nothing to move - Add.");
    }

    var Voptionx = <HTMLSelectElement>document.getElementById("Variable_Modifications");
    var len = selectedOpts.length
    for (let i = 0; i < len; i++) {
      Voptionx.append(selectedOpts[i]);
    }
  }

  remove_mod() {
    var optionx = <HTMLSelectElement>document.getElementById("Variable_Modifications");
    var selectedOpts = optionx.selectedOptions;
    if (selectedOpts.length == 0) {
      alert("If it is not already empty then, please select type of modification you want to remove from Variable Modications.");
      // alert("Nothing to move - Add.");
    }
    var Voptionx = <HTMLSelectElement>document.getElementById("List_of_Modifications");
    var len = selectedOpts.length
    for (let i = 0; i < len; i++) {
      Voptionx.append(selectedOpts[i]);
    }
  }

  VariableModification(): string  //For Form Data (Json) to API
  {
    var VarModArray : string[] = [];
    var VariableModOption = (<HTMLSelectElement>document.getElementById("Variable_Modifications"));

    for (let i =0; i < VariableModOption.length; i++){
      VarModArray.push(VariableModOption[i].innerHTML);
    }
    return VarModArray.toString();
  }

  
  FixedModification(): string  //For Form Data (Json) to API
  {
    var FixedModArray : string[] = [];
    var FixedModOption = (<HTMLSelectElement>document.getElementById("Fixed_Modification"));

    for (let i =0; i < FixedModOption.length; i++){
      FixedModArray.push(FixedModOption[i].innerHTML);
    }
    return FixedModArray.toString();
  }


  onSubmit(form: any): void {
    var user = firebase.auth().currentUser;

    // #JUSTNEEDED: MAY HAVE BETTER SOLUTION: Converting string Array into string...!!!
    form.HandleIons= form.HandleIons.toString();
    form.TerminalModification = form.TerminalModification.toString();

    if (form.MassMode == 1){
      form.MassMode = "MH+";
    }
    else{
      form.MassMode = "M(Neutral)";
    }
    
    //Adding Variable Modifications into form
    if (<HTMLSelectElement>document.getElementById("Variable_Modifications") != null){
      form.VariableModifications = this.VariableModification();
    }
    //Adding Fixed Modifications into form
    if (<HTMLSelectElement>document.getElementById("Fixed_Modification") != null){
      form.FixedModifications = this.FixedModification();
    }
  

    if (user.emailVerified == true) {
      form.EmailId = user.email;
      form.UserId = user.email;
    }
    else{
      // form.UserId = user.uid;
      if (form.UserId != ""){
        form.EmailId = form.UserId;
        form.UserId = user.uid;
      }
      else{
        form.EmailId = "";
        form.UserId = user.uid;
      }
    }

    if(form.MwSweight == ""){
      form.MwSweight = 0;
    } 
    if (form.PstSweight == ""){
      form.PstSweight = 0; 
    }
    if (form.InsilicoSweight == ""){
      form.InsilicoSweight = 0;
    }

    if(form.MwSweight == "0" && form.PstSweight == "0" && form.InsilicoSweight == "0"){
      alert("Dear User! You did not select any weightage from 'Set Scoring Components Weight'. \nSo, PERCEPTRON will select Spectral Comparisons Score Weightage (100%) by default.");
      form.InsilicoSweight = 100;
    }
    if(Number(form.MaximumPstLength) < Number(form.MinimumPstLength)){
      form.MaximumPstLength = 6;
      form.MinimumPstLength = 3;
      alert("Dear User! \nYour selected value for Minimum Tag Length and Maximum Tag Length is not appropriate. \nMaximum Tag Length should be greater than Minimum Tag Length. \n So, PERCEPTRON will select Minimum Tag Length as 3 and Maximum Tag Length as 6 by default.")
    }

    // if (form.DeconvAllow == true) {
    //   form.DeconvAllow = 1;
    // }
    // else {
    // form.DeconvAllow = 0;
    // }

    // if (form.FilterDb == true) {
    //   form.FilterDb = 1;
    // }
    // else {
    //   form.FilterDb = 0;
    // }

    // if (form.Truncation == true) {  // Trunc REPLACED WITH Truncation
    //   form.Truncation = 1;
    // }
    // else {
    //   form.Truncation = 0;
    // }


    // if (form.Autotune == true) {
    //   form.Autotune = 1;
    // }
    // else {
    //   form.Autotune = 0;
    // }

    // if (form.DenovoAllow == true) {
    //   form.DenovoAllow = 1;
    // }
    // else {
    //   form.DenovoAllow = 0;
    // }

    // if(<HTMLSelectElement>document.getElementById("Variable_Modifications") != null || <HTMLSelectElement>document.getElementById("Fixed_Modifications") != null || form.PtmAllow == true){

    //   //form.PtmAllow = 1;

    //   if(form.Methionine_ChemicalModification == ""){
    //     form.Methionine_ChemicalModification = "None";
    //   }
    //   if(form.Cysteine_ChemicalModification == ""){
    //     form.Cysteine_ChemicalModification = "None";
    //   }
    // }
    // else{
    //   form.PtmAllow = 0;
    // }
  

    let fi = this.imgFileInput.nativeElement;
    let stats: any = 'false';
    console.log(form);
    stats = this._httpService.postJSON(form, fi.files);
    console.log(stats)
    alert("Dear User! \nYour search query has been submitted for results either visit 'Search Results & History' tab and/or check your email. \n\nThank You for using PERCEPTRON. \nThe PERCEPTRON Team");
  }

  onReset(form: any): void {
    console.log("Form has been reset");
  }
}


