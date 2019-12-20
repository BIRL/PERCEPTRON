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
  selector: 'app-update-username',
  templateUrl: './update-username.component.html',
  styleUrls: ['./update-username.component.css']
})
export class UpdateUsernameComponent implements OnInit {
  error:any;
  hide:any;
  username:any;
  constructor(public myapp: AppComponent, public af: AngularFireAuth, private router: Router) { }
  onSubmit(formData) {
    if (formData.valid) {
      var user = firebase.auth().currentUser;
      var x = this.router;
      var username = formData.value.username;
      user.updateProfile({
        displayName: username,
        photoURL: ""
      }).then((success) => {
        localStorage.setItem('logged_in_user', username);
        this.myapp.logged_in_user = username;
        alert('successfully updated')
        x.navigate(['/settings']);
      }).catch((err) => {
        var errorCode = err.code;
        var errorMessage = err.message;
        alert(errorMessage);
      });
    }
  }
  ngOnInit() {
  }

}
