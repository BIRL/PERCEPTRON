import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ConfigService } from '../config.service';

@Component({
  selector: 'app-results-visualization',
  templateUrl: './results-visualization.component.html',
  styleUrls: ['./results-visualization.component.css'],
  providers: [ConfigService]
})
export class ResultsVisualizationComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
