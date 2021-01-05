import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AngularFireAuth } from 'angularfire2/auth';
import { Router } from '@angular/router';
import { moveIn, fallIn } from '../router.animations';
import {MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule, MatMenuModule, MatInputModule} from '@angular/material';
@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {

  email = new FormControl('', [Validators.required, Validators.email]);
  getErrorMessage() {
    return this.email.hasError('required') ? 'You must enter a value' :
        this.email.hasError('email') ? 'Not a valid email' :
            '';
  }
  hide = true;

  state: string = '';
  error: any;

  constructor(public af: AngularFireAuth, private router: Router) { }
  
  onSubmit(formData) {
    if (formData.valid) {
      var x = this.router;
      console.log(formData.value);
      var email = formData.value.email
      this.resetPassword(email, x)
    }
  }


  resetPassword(email: string, x: any) {
    this.af.auth.sendPasswordResetEmail(email).then((success) => {
      alert('An email with password reset instructions has been sent.');
      x.navigate(['/login']);
    }).catch((err) => {
      var errorCode = err.code;
      var errorMessage = err.message;
      this.error = err;
    });
  }


  ngOnInit() {
  }

}
