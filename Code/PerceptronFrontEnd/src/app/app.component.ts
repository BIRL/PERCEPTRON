import { Component, OnInit, ViewChild } from '@angular/core';
import { MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule } from '@angular/material';
import { AngularFireAuth } from 'angularfire2/auth';
import { Router } from '@angular/router';
import * as firebase from 'firebase/app';
import { Alert } from 'selenium-webdriver';
import { find } from 'rxjs/operators';
//import { userInfo } from 'os';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  // onMainEvent: EventEmitter = new EventEmitter();
  title = 'app';
  logged_in_user: any;
  show: any;
  loginButtonVisibility: any;
  @ViewChild('my') my1;
  sidebutton: any = '';
  UserEmailID: any;  //For show/hide Admin Panel
  // menubutton: boolean;


  disabled: boolean;
  disabled1: boolean;
  netImage: any = "../assets/images/fhh1.png";
  @ViewChild("menubutton") menubutton;
  buttonDisabled: any;
  constructor(public af: AngularFireAuth, private router: Router) {
    this.check();
  }
  isOn: boolean;

  tables = [
    { id: '1', Dataset: 'hela', Description: 'Hi', Download: 'Click' },
    { id: '2', Dataset: 'hela2', Description: 'Hi', Download: 'Click' },
    { id: '3', Dataset: 'hela3', Description: 'Hi', Download: 'Click' },
    { id: '4', Dataset: 'hela4', Description: 'Hi', Download: 'Click' },
    { id: '5', Dataset: 'hela5', Description: 'Hi', Download: 'Click' },


    { id: '1', Dataset: 'hela', Description: 'Hi', Download: 'Click' },
    { id: '2', Dataset: 'hela2', Description: 'Hi', Download: 'Click' },
    { id: '3', Dataset: 'hela3', Description: 'Hi', Download: 'Click' },
    { id: '4', Dataset: 'hela4', Description: 'Hi', Download: 'Click' },
    { id: '5', Dataset: 'hela5', Description: 'Hi', Download: 'Click' },

  ];

  login() {
    this.router.navigate(['/login']);
  }

  perceptron() {
    var user = firebase.auth().currentUser;

    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/home']);
    } else {
      this.router.navigate(['/home']);
    }
  };

  about() {
    var user = firebase.auth().currentUser;
    if (user != null) {
      this.UserEmailID = user.email;
    }
    this.router.navigate(['/about']);
  };

  getting_started() {
    var user = firebase.auth().currentUser;
    if (user != null) {
      this.UserEmailID = user.email;
    }

    this.router.navigate(['/getting']);    // /about
  };
  //  {
  //   var user = firebase.auth().currentUser;
  //   if (user) {
  //     this.router.navigate(['/getting']);
  // } else {
  //   alert("Kindly login first!")
  //   this.router.navigate(['/login']);
  //   };
  //  };

  search() {
    var user = firebase.auth().currentUser;

    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/search']);
      // this.disabled=false;
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);
      // this.disabled=false;
    };
  };

  history() {
    var user = firebase.auth().currentUser;

    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/history']);
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);
    };
  };

  ResultsInterpretation() {
    var user = firebase.auth().currentUser;
    this.UserEmailID = user.email;
    this.router.navigate(['/ResultsInterpretation']);
  };

  fdr() {
    var user = firebase.auth().currentUser;
    this.UserEmailID = user.email;
    this.router.navigate(['/fdr']);
  };

  data() {  // Sample Data
    var user = firebase.auth().currentUser;

    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/repository']);
    } else {
      alert("Kindly login or use as a guest!");
      this.router.navigate(['/login']);
    }
  };

  help() {
    var user = firebase.auth().currentUser;
    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/help']);
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);

    };
  }

  youtube() {  // Video Tutorials
    var user = firebase.auth().currentUser;
    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/youtube']);
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);
    };
  };

  bug() {
    var user = firebase.auth().currentUser;

    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/bug-report']);
    } else {
      alert("Kindly login or use as a guest!");
      this.router.navigate(['/login']);
    }
  };
  team() {
    var user = firebase.auth().currentUser;
    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/team']);
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);
    };
  };
  contact() {
    var user = firebase.auth().currentUser;

    if (user) {
      this.UserEmailID = user.email;
      this.router.navigate(['/contact']);
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);
    };
  }

  adminpanel() {
    var user = firebase.auth().currentUser;
    if (user.email == "farhan.khalid@lums.edu.pk") {
      this.router.navigate(['/adminpanel']);
    } else {
      alert("Dear User! You have restricted access.")
      // this.router.navigate(['/login']);

    };
  }

  check() {
    var user = firebase.auth().currentUser;

    if (user) {
      this.disabled = true;
      this.disabled1 = false;

    } else {
      this.disabled = false;
      this.disabled1 = true;

    };
  };
  update_password() {
    alert('a');
    var user = firebase.auth().currentUser;
    if (user) {
      this.router.navigate(['/update-password']);
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);
    };
  };

  ngOnInit() {

    var logged_in = localStorage.getItem('login');
    if (logged_in) {
      this.disabled = true;
      if (localStorage.getItem('logged_in_user')) {
        this.logged_in_user = localStorage.getItem('logged_in_user');
      }
      else {
        this.logged_in_user = 'user';
      }
      this.disabled1 = false;
    } else {
      this.disabled = false;
      this.disabled1 = true;
    }
  }
  okk() {
    alert("in okk");
    this.disabled = true;
  }




  account() {
    var user = firebase.auth().currentUser;
    if (user.emailVerified == true) {
      if (confirm("Do you want to logout?")) {
        this.af.auth.signOut();
        this.disabled = false;
        this.disabled1 = true;
        localStorage.removeItem('login');
        localStorage.removeItem('logged_in_user');
        this.UserEmailID = "";
        this.router.navigateByUrl('/home');
      }
    }

    else if (confirm("Warning:\nYou are logged in as a Guest. Closing this window or logging out will result in the loss of your search results.")) {
      this.af.auth.signOut();
      this.disabled = false;
      this.disabled1 = true;
      localStorage.removeItem('login');
      localStorage.removeItem('logged_in_user');
      this.UserEmailID = "";
      this.router.navigateByUrl('/home');
    }
  };

  setting() {
    var user = firebase.auth().currentUser;
    if (user) {
      this.router.navigate(['/settings']);
    } else {
      alert("Kindly login or use as a guest!");
      this.router.navigate(['/login']);
    }

  };
  //Now, Added...!!!
  toggle = true;

  EnableDisableButtonColor(job) {
    this.toggle = !this.toggle;
  }




  demo() {
    this.router.navigate(['/demo']);
  };
  hudiara() {
    var user = firebase.auth().currentUser;
    if (user) {
      this.router.navigate(['/hudiara']);
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);
    };

  }
  maps() {
    var user = firebase.auth().currentUser;
    if (user) {
      this.router.navigate(['/maps']);
    } else {
      alert("Kindly login or use as a guest!")
      this.router.navigate(['/login']);
    };
  };
  patterngenerator() {

    var user = firebase.auth().currentUser;
    if (user) {
      this.router.navigate(['/patterngenerator']);
    } else {
      alert("Kindly login first!")
      this.router.navigate(['/login']);
    };
  }

}



