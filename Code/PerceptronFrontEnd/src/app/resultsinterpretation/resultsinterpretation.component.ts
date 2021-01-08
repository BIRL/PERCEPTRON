import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-resultsinterpretation',
  templateUrl: './resultsinterpretation.component.html',
  styleUrls: ['./resultsinterpretation.component.css']
})
export class ResultsinterpretationComponent implements OnInit {

  constructor() { }

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

}
