import { Component, OnInit, ViewChild } from '@angular/core';
import { MatToolbarModule, MatSidenavModule, MatCardModule, MatButtonModule, MatIconModule } from '@angular/material';
import { AngularFireAuth } from 'angularfire2/auth';
import { Router } from '@angular/router';
import * as firebase from 'firebase/app';
import { Alert } from 'selenium-webdriver';
import { find } from 'rxjs/operators';


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
  // menubutton: boolean;
  
  
  disabled: boolean;
  disabled1: boolean;
  netImage:any = "../assets/images/fhh1.png";
  @ViewChild("menubutton") menubutton;
  buttonDisabled:any;
  constructor(public af: AngularFireAuth,private router: Router) {
    this.check();  
    
   }
  isOn: boolean;

  tables = [
    { id: '1', Dataset: 'hela', Description: 'Hi', Download: 'Click' },
    { id: '2', Dataset: 'hela2' , Description: 'Hi', Download: 'Click' },
    { id: '3', Dataset: 'hela3', Description: 'Hi', Download: 'Click' },
    { id: '4', Dataset: 'hela4' , Description: 'Hi', Download: 'Click'},
    { id: '5', Dataset: 'hela5', Description: 'Hi', Download: 'Click' },


    { id: '1', Dataset: 'hela', Description: 'Hi', Download: 'Click' },
    { id: '2', Dataset: 'hela2' , Description: 'Hi', Download: 'Click' },
    { id: '3', Dataset: 'hela3', Description: 'Hi', Download: 'Click' },
    { id: '4', Dataset: 'hela4' , Description: 'Hi', Download: 'Click'},
    { id: '5', Dataset: 'hela5', Description: 'Hi', Download: 'Click' },
  
  ];

    about() {
     this.router.navigate(['/about']);
    };

    youtube() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/youtube']);
    } else {
      alert("Kindly login first!")
      this.router.navigate(['/login']);
      };
     };

     getting_started() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/getting']);
    } else {
      alert("Kindly login first!")
      this.router.navigate(['/login']);
      };
     };
 

    history() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/history']);
    } else {
        alert("Kindly login first!")
        this.router.navigate(['/login']);
      };
     };



check(){
  var user = firebase.auth().currentUser;

      if (user) {
        this.disabled=true;
        this.disabled1=false;
        
    } else {
      this.disabled=false;
      this.disabled1=true;
    
      };
     };

     team() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/team']);
    } else {
    alert("Kindly login first!")
    this.router.navigate(['/login']);
      };
     };

    help() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/help']);
    } else {
    alert("Kindly login first!")
    this.router.navigate(['/login']);
     
    };
  }

  contact() {
    var user = firebase.auth().currentUser;
    if (user) {
      this.router.navigate(['/contact']);
  } else {
  alert("Kindly login first!")
  this.router.navigate(['/login']);
   
  };
}
   


    demo() {
      this.router.navigate(['/demo']);
    };
    hudiara(){
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/hudiara']);
    } else {
    alert("Kindly login first!")
    this.router.navigate(['/login']);
    };
      
    }
    maps() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/maps']);
    } else {
    alert("Kindly login first!")
    this.router.navigate(['/login']);
    };
  };

  update_password() {
    alert('a');
    var user = firebase.auth().currentUser;
    if (user) {
      this.router.navigate(['/update-password']);
  } else {
  alert("Kindly login first!")
  this.router.navigate(['/login']);
  };
};

search() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/search']);
        // this.disabled=false;
    } else {
    alert("Kindly login first!")
    this.router.navigate(['/login']);
    // this.disabled=false;
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



    
    login() {
      this.router.navigate(['/login']);
    }

    perceptron(){
      var user = firebase.auth().currentUser;
      if (user) {
      this.router.navigate(['/home']);
    } else {
      this.router.navigate(['/home']);
    }
    };

    ngOnInit() {
    
     var logged_in = localStorage.getItem('login');
     if (logged_in){
      this.disabled=true;
      if (localStorage.getItem('logged_in_user')){
        this.logged_in_user = localStorage.getItem('logged_in_user');
      }
      else{
        this.logged_in_user = 'user';
      }
      this.disabled1=false;
     } else{
      this.disabled=false;
      this.disabled1=true;
      }
     
      
    }
    okk(){
      alert("in okk");
      this.disabled=true;
    }

    data() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/repository']);
    } else {
      alert("Kindly login first!");
      this.router.navigate(['/login']);
    }
      
    };
  
    bug() {
      var user = firebase.auth().currentUser;
      if (user) {
        this.router.navigate(['/bug-report']);
    } else {
      alert("Kindly login first!");
      this.router.navigate(['/login']);
    }
      
   
    };
    account() {
      if (confirm("Do you want to logout?"))
      {
        this.af.auth.signOut();
        this.disabled=false;
        this.disabled1=true;
        localStorage.removeItem('login');
        localStorage.removeItem('logged_in_user');
        this.router.navigateByUrl('/home');
      }
      };

      setting(){
        var user = firebase.auth().currentUser;
        if (user) {
          this.router.navigate(['/settings']);
      } else {
        alert("Kindly login first!");
        this.router.navigate(['/login']);
      }

      };
      
      //Now, Added...!!!
      toggle = true;

      EnableDisableButtonColor(job){
        this.toggle = !this.toggle;
      }

  }



