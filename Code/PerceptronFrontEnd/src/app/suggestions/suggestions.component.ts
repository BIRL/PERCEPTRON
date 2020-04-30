import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AngularFireAuth } from 'angularfire2/auth';

@Component({
  selector: 'app-suggestions',
  templateUrl: './suggestions.component.html',
  styleUrls: ['./suggestions.component.css']
})
export class SuggestionsComponent implements OnInit {

  constructor(public af: AngularFireAuth, private router: Router) { }

  ngOnInit() {
  }


  onSubmit(form: any): void {
    console.log('you submitted value:', JSON.stringify(form));
    alert("Thank you for submitting your suggestion!");
  
  }

}
