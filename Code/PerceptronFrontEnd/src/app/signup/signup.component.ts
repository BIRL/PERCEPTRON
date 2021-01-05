import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import {MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule, MatMenuModule, MatInputModule} from '@angular/material';
import { AngularFireAuth } from 'angularfire2/auth';
import { Router } from '@angular/router';
import { moveIn, fallIn } from '../router.animations';
import {AppComponent} from '../app.component';
@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
  // animations: [moveIn(), fallIn()],
  // host: { '[@moveIn]': '' }
})
export class SignupComponent implements OnInit {

  email = new FormControl('', [Validators.required, Validators.email]);
  getErrorMessage() {
    return this.email.hasError('required') ? 'You must enter a value' :
        this.email.hasError('email') ? 'Not a valid email' :
            '';
  }
  hide = true;
  user_name:any;
  state: string = '';
  error: any;
  name: any;
  password: any;
  password2:any;
  passcheck :any;
  constructor(public myapp: AppComponent,public af: AngularFireAuth, private router: Router) {

  }

  onSubmit(formData) {
    var val =  (<HTMLInputElement>document.getElementById("password")).value;
    var val2 =  (<HTMLInputElement>document.getElementById("password2")).value;
    if(val == val2){
    if (formData.valid) {
      var email = formData.value.email
      var password = formData.value.password;
      var user_name = formData.value.name;
      var x = this.router;

      this.af.auth.createUserWithEmailAndPassword(email, password) //Register User
      .then((user) => {
        //console.log("user registered");

        return this.af.auth.currentUser;
      })
      .then((user) => {
        //console.log("get user");
        return user.updateProfile({ displayName: this.name, photoURL: "" });
      })
      .then(() => {
        //console.log("Profile Updated");
        var user = this.af.auth.currentUser;
        console.log(user);
        return user.sendEmailVerification();
      })
      .then(() => {
        //console.log("Email sent");
        return this.af.auth.signOut();
      })
      .then(() => {
        console.log("signed out");
        alert("Dear User,\nA verification email will be sent to your email address shortly. Please verify your email address before you login.\n\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team");
        x.navigate(['/login']) 
        // .push({
        //   name: "registered",
        //   params: { userName: this.name, email: this.email },
        // });
      })
      .catch((errors) => {
        
        console.log(errors);
        // ..
      });


      // this.af.auth.createUserWithEmailAndPassword(email, password).
      //   then((success) => {
      //     this.af.authState.subscribe(user => {user.updateProfile({
      //       displayName: user_name,
      //       photoURL: ""
      //     })})
      //     this.myapp.disabled1=false;
      //     this.myapp.disabled=true;
      //     this.myapp.logged_in_user = user_name;
      //     localStorage.removeItem('login');
      //     localStorage.removeItem('logged_in_user');
      //     // localStorage.setItem('login','1');   //Updated 20201219
      //     // localStorage.setItem('logged_in_user', user_name);   //Updated 20201219
      //     x.navigate(['/login'])    //Updated 20201219
      //     alert("Dear User,\nA verification email will be sent to your email address shortly. Please verify your email address before you login.\n\nThank you for using PERCEPTRON!\nThe PERCEPTRON Team")
      //   }).catch((err) => {
      //     var errorCode = err.code;
      //     var errorMessage = err.message;
      //     if (errorCode == 'auth/weak-password') {
      //       alert('The password is too weak.');
      //     } else {
      //       // alert(errorMessage);
      //     }
      //     console.log(err);
      //     this.error = err;
      //   });
    } }
    else{
      alert("Passwords must match");
    }
  }
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

  
  // }
}
