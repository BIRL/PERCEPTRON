import { CanActivate, Router } from '@angular/router';
import { AngularFireAuth } from 'angularfire2/auth';
import * as firebase from 'firebase/app';

import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Rx";

import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/take';

@Injectable()
export class AuthGuard implements CanActivate {
  // private authState: Observable<firebase.User>
  // private currentUser: firebase.User = null;

  constructor(private auth: AngularFireAuth, private router: Router) {}
  //   constructor(private auth: AngularFireAuth, private router: Router) {
  //     this.authState = this.auth.authState;
  //     this.authState.subscribe(user => {
  //       if (user) {
  //         this.currentUser = user;
  //       } else {
  //         this.currentUser = null;
  //       }});
  //   }

  //   getAuthState() {
  //     return this.authState;
  //   }
      
    // resetPassword(email: string) {
    //   debugger
    //   var auth = firebase.auth();
    //   return auth.sendPasswordResetEmail(email)
    //     .then(() => console.log("email sent"))
    //     .catch((error) => console.log(error))
    // }


    canActivate() 
    : Observable<boolean> {
      return Observable.from(this.auth.authState)
        .take(1)
        .map(state => !!state)
        .do(authenticated => {
      if 
        (!authenticated) this.router.navigate([ '/login' ]);
      })
    }
 
}