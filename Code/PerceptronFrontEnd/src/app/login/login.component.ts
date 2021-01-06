import { Component, OnInit } from '@angular/core';
import { AngularFireAuth } from 'angularfire2/auth';
import { Router } from '@angular/router';
import { moveIn } from '../router.animations';
import * as firebase from 'firebase/app';
import { AppComponent } from '../app.component';
import { Output, EventEmitter } from '@angular/core';
import { MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule } from '@angular/material';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
  // animations: [moveIn()],
  // host: { '[@moveIn]': '' }
})
export class LoginComponent implements OnInit {
  @Output() observableEvent: EventEmitter<any> = new EventEmitter<any>();
  error: any;
  constructor(public myapp: AppComponent, public af: AngularFireAuth, private router: Router) {
    this.af.authState.subscribe(user => { })
  }



  loginFb() {
    var provider = new firebase.auth.FacebookAuthProvider();
    provider.addScope('email');
    var x = this.router;
    var login = firebase.auth().signInWithPopup(provider);
    login.then(function (result) {
      // This gives you a Facebook Access Token.
      // var token = result.credential.accessToken;
      // The signed-in user info.
      // var user = result.user;
      x.navigate(['/search']);
    }).catch(
      (err) => {
        alert(err);
        this.error = err;
      })
  };

  loginGoogle() {
    var provider = new firebase.auth.GoogleAuthProvider();
    var x = this.router;
    firebase.auth().signInWithPopup(provider).then(function (result) {
      // This gives you a Google Access Token.
      // var token = result.credential.accessToken;
      // The signed-in user info.
      // var user = result.user;
      localStorage.removeItem('login');
      localStorage.removeItem('logged_in_user');
      alert("Dear User,\nYour account has been successfully created. Please login with your credentials.\n\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team");

      x.navigate(['/login']);
    }).catch(
      (err) => {
        this.error = err;
      })
  };

  loginGuest() {
    var x = this.router;
    var y = this.myapp;
    firebase.auth().signInAnonymously().then(function (result) {
      localStorage.setItem('login', '1');
      localStorage.setItem('logged_in_user', 'guest')
      y.disabled = true;
      y.logged_in_user = localStorage.getItem('logged_in_user')
      y.disabled1 = false;
      x.navigate(['/search']);
    }).catch(
      (err) => {
        this.error = err;
        alert(err);
      })


  };


  ngOnInit() {
    this.myapp.disabled = true;
  }
  ngOnDestroy() {
    if (this.myapp.disabled1) {
      this.myapp.disabled = false;
    }
    else {
      this.myapp.disabled = true;
    }
  }
}
