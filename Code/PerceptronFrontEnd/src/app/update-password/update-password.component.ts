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
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrls: ['./update-password.component.css']
})
export class UpdatePasswordComponent implements OnInit {
  username:any;
  error:any;
  hide:any;
  password:any;
  confirm_password:any;
  constructor(public myapp: AppComponent, public af: AngularFireAuth, private router: Router) {
    var x = this.router;
    this.af.authState.subscribe
      (user => {
        // x.navigateByUrl('/yoohoo');
      })
  }
  onSubmit(formData) {
    if (formData.valid) {
      var user = firebase.auth().currentUser;
      var x = this.router;
      var confirm_password = formData.value.confirm_password;
      var password = formData.value.password;
      if (password !== confirm_password) {
        alert('Passwords do not match.');
      }else{
        user.updatePassword(password).then((success) => {
          alert('successfully updated')
          x.navigate(['/settings']);
        }).catch((err) => {
          var errorCode = err.code;
          var errorMessage = err.message;
          alert(errorMessage);
        });
      }
    }
  }
  
  ngOnInit() {
  }

}
