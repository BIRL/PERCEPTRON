import {MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule, MatMenuModule, MatInputModule} from '@angular/material';
import { Component, OnInit } from '@angular/core';
import { AngularFireAuth } from 'angularfire2/auth';
import { Router } from '@angular/router';
import * as firebase from 'firebase/app';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css']
})
export class ToolbarComponent implements OnInit {

  constructor(public af: AngularFireAuth,private router: Router) { }

  about() {
    this.router.navigate(['/about']);
  };

  search() {
    this.router.navigate(['/search']);
  };

  bug() {
    this.router.navigate(['/bug-report']);
  };

  account() {
    this.af.auth.signOut();
    console.log('logged out');
    this.router.navigateByUrl('/login');
  };

  ngOnInit() {
  }

}
