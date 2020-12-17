import { router } from './../app.routes';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import {MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule, MatMenuModule, MatInputModule} from '@angular/material';
import {AppComponent} from '../app.component';
import { AngularFireAuth } from 'angularfire2/auth';
import * as firebase from 'firebase/app';

import { Router } from '@angular/router';
import { moveIn, fallIn } from '../router.animations';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  constructor(public myapp: AppComponent, public af: AngularFireAuth, private router: Router) {
    var x = this.router;
    this.af.authState.subscribe
      (user => {
        // x.navigateByUrl('/yoohoo');
      })
  }
  update_password() {
      var user = firebase.auth().currentUser;
      if (user.isAnonymous){
        alert('Not authorized, kindly login with email to access this functionality.');
      }
      else if (user) {
        this.router.navigate(['/update-password']);
    } else {
    alert("Kindly login first!")
    this.router.navigate(['/login']);
    };
  };

  
  update_username() {
    var user = firebase.auth().currentUser;
    if (user.isAnonymous){
      alert('Not authorized, kindly login with email to access this functionality.');
    }
    else if (user) {
      this.router.navigate(['/update-username']);
  } else {
  alert("Kindly login first!")
  this.router.navigate(['/login']);
  };
  };
  
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
