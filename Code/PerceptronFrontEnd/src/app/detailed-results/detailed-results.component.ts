import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';


@Component({
  selector: 'app-detailed-results',
  templateUrl: './detailed-results.component.html',
  styleUrls: ['./detailed-results.component.css'],
  providers: [ConfigService]
})
export class DetailedResultsComponent implements OnInit {
  displayedColumns = ['sr', 'id', 'name', 'exp', 'thr', 'diff'];
  dataSource: MatTableDataSource<UserData>;
  querryId: any;
  resultId: any;
  rank: any;
    constant: any;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;



  constructor(private route: ActivatedRoute, private router: Router, private _httpService: ConfigService) {
    // Create 100 users
    const users: UserData[] = [];
    this.dataSource = new MatTableDataSource(users);
  }

  // goToPage(pageName:string):void{
  //   this.router.navigate([`${pageName}`])
  // };

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
    this.route.params.subscribe((params: Params) => this.resultId = params['resultId']);
    this.route.params.subscribe((params: Params) => this.rank = params['rank']);
    this._httpService.GetDetailedReslts(this.resultId).subscribe(data => this.what(data));
  }

  what(data: any) {
    const users: UserData[] = [];
    for (let i = 1; i <= data.length; i++) { users.push(createNewUser(i, data[i - 1])); }
    this.dataSource = new MatTableDataSource(users);
    
    
    this.constant="http://www.uniprot.org/uniprot/"+data.Results.Results.Header;
    
    let ProteinRank = <HTMLLabelElement>document.getElementById("ProteinRank");
    let ProteinID = <HTMLLabelElement>document.getElementById("ProteinID");
    let ProteinName = <HTMLLabelElement>document.getElementById("ProteinName");
    let ProteinScore = <HTMLLabelElement>document.getElementById("ProteinScore");
    let MolW = <HTMLLabelElement>document.getElementById("MolW");
    let MatchedFrags = <HTMLLabelElement>document.getElementById("MatchedFrags");
    let TermMods = <HTMLLabelElement>document.getElementById("TermMods");
    let TermMod = <HTMLLabelElement>document.getElementById("TermMod");
    let Truncation = <HTMLLabelElement>document.getElementById("Truncation"); //
    let Mods = <HTMLLabelElement>document.getElementById("Mods");
    let totalTime = <HTMLLabelElement>document.getElementById("totalTime");
    let MWModule = "N/A";
    let PSTModule = <HTMLLabelElement>document.getElementById("PSTModule");
    let InsilicoModule = <HTMLLabelElement>document.getElementById("InsilicoModule");
    let PTMModule = <HTMLLabelElement>document.getElementById("PTMModule");
    let TruncationModule = <HTMLLabelElement>document.getElementById("TruncationModule");
    let massmode = <HTMLLabelElement>document.getElementById("MassMode");
    let BPTMS = <HTMLLabelElement>document.getElementById("BPTMs");

    massmode.innerHTML = "MH+";
    BPTMS.innerHTML = "No";


    let title = <HTMLLabelElement>document.getElementById("SearchTitle");
    title.innerHTML = data.Paramters.SearchParameters.Title;
   
    let pdb = <HTMLLabelElement>document.getElementById("ProteinDB");
    pdb.innerHTML = data.Paramters.SearchParameters.ProtDb;


    let protTol = <HTMLLabelElement>document.getElementById("protTol");
    protTol.innerHTML = data.Paramters.SearchParameters.MwTolerance;

    let autotunee = <HTMLLabelElement>document.getElementById("Tuner");            
    autotunee.innerHTML = data.Paramters.SearchParameters.Autotune;

    let ppeptol = <HTMLLabelElement>document.getElementById("peptol");            
    ppeptol.innerHTML = data.Paramters.SearchParameters.HopThreshhold;  

    let fragt = <HTMLLabelElement>document.getElementById("FragType");            
    fragt.innerHTML = data.Paramters.SearchParameters.InsilicoFragType; 

    let SpecI = <HTMLLabelElement>document.getElementById("SI");            
    SpecI.innerHTML = data.Paramters.SearchParameters.HandleIons; 

    let DenovAllow = <HTMLLabelElement>document.getElementById("PST");            
    DenovAllow.innerHTML = data.Paramters.SearchParameters.DenovoAllow;

    let PstLength = <HTMLLabelElement>document.getElementById("PSTLen");            
    PstLength.innerHTML = data.Paramters.SearchParameters.MinimumPstLength + " " + data.Paramters.SearchParameters.MaximumPstLength ;

    let IPMSWeight = <HTMLLabelElement>document.getElementById("Slider1");
    let PSTWeight = <HTMLLabelElement>document.getElementById("Slider2");
    let SpecCompWeight = <HTMLLabelElement>document.getElementById("Slider3");

    IPMSWeight.innerHTML = data.Results.Results.MwScore;
    PSTWeight.innerHTML = data.Results.Results.PstScore;
    SpecCompWeight.innerHTML = data.Results.Results.InsilicoScore;


    ProteinRank.innerHTML = this.rank;
    ProteinID.innerHTML = data.Results.Results.Header;
    ProteinName.innerHTML = data.Results.Results.Header;
    ProteinScore.innerHTML = data.Results.Results.Score.toFixed(6);
    MolW.innerHTML = data.Results.Results.Mw.toFixed(6);
    MatchedFrags.innerHTML = "12";
    TermMods.innerHTML = "NME";
    TermMod.innerHTML = "NME";
    Truncation.innerHTML = "No";
    Mods.innerHTML = "3";
    let constant = ProteinID.innerHTML;
  let met = constant;

    totalTime.innerHTML = data.ExecutionTime.TotalTime;
    MWModule = data.ExecutionTime.MwFilterTime;
    PSTModule.innerHTML = data.ExecutionTime.PstTime;
    InsilicoModule.innerHTML = data.ExecutionTime.InsilicoTime;
    PTMModule.innerHTML = data.ExecutionTime.PtmTime;
    // add later
    TruncationModule.innerHTML = data.ExecutionTime.TotalTime;

    let sequence = <HTMLLabelElement>document.getElementById("sequence");
    let sequenceText = data.Results.Results.Sequence;
    let text = sequenceText[0];
    let a=1;
    
    for (let i = 1; i < sequenceText.length; i++) {
      
      
      if (i % 10 == 0) {
        ++a;
        text = text + "\xa0\xa0\xa0\xa0\xa0" + a+". " + sequenceText[i];
      }
      else {
        if (i==1){
          text ="1. "+ text + "" + sequenceText[i];
        }else
        {
        text = text + "" + sequenceText[i];
      }
      }
    }
    sequence.innerHTML = text;
    
  }


  // ResultsView(pageName:string):void{
  //   this.router.navigate([`${pageName}`])
  // }

  
  getResultsView() {
    let x = this.router;
    x.navigate(["resultsvisualization", this.resultId, this.rank]);   //, ResultId
  }

}
 



/** Builds and returns a new User. */
function createNewUser(id: number, data: any): UserData {

  return {
    serial: id.toString(),
    name: id.toString(),
    id: id.toString(),
    molW: id.toString(),
    term: id.toString(),
    mods: id.toString()
  };
}

export interface UserData {
  serial: string;
  name: string;
  id: string;
  molW: string;
  term: string;
  mods: string;
}
