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
      this.af.auth.createUserWithEmailAndPassword(email, password).
        then((success) => {
          this.af.authState.subscribe(user => {user.updateProfile({
            displayName: user_name,
            photoURL: ""
          })})
          this.myapp.disabled1=false;
          this.myapp.disabled=true;
          this.myapp.logged_in_user = user_name;
          localStorage.setItem('login','1');
          localStorage.setItem('logged_in_user', user_name);
          x.navigate(['/search'])
        }).catch((err) => {
          var errorCode = err.code;
          var errorMessage = err.message;
          if (errorCode == 'auth/weak-password') {
            alert('The password is too weak.');
          } else {
            // alert(errorMessage);
          }
          console.log(err);
          this.error = err;
        });
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
