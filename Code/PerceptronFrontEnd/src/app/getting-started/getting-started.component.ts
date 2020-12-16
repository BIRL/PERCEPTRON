import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-getting-started',
  templateUrl: './getting-started.component.html',
  styleUrls: ['./getting-started.component.css']
})
export class GettingStartedComponent implements OnInit {
  constructor() { }

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
  // clicked1(){
  //   var size= document.getElementById('get1')
  //   if (size.getAttribute('width') == "100%"){
  //     size.setAttribute('width',"80%")
  //   }else{
  //     size.setAttribute('width',"100%")
  //   }
  // }
  // clicked2(){
  //   var size= document.getElementById('get2')
  //   if (size.getAttribute('width') == "100%"){
  //     size.setAttribute('width',"60%")
  //   }else{
  //     size.setAttribute('width',"100%")
  //   }
  // }

  //   clicked3(){
  //     var size= document.getElementById('get3')
  //     if (size.getAttribute('width') == "100%"){
  //       size.setAttribute('width',"60%")
  //     }else{
  //       size.setAttribute('width',"100%")
  //     }
  //   }

  //     clicked4(){
  //       var size= document.getElementById('get4')
  //       if (size.getAttribute('width') == "100%"){
  //         size.setAttribute('width',"60%")
  //       }else{
  //         size.setAttribute('width',"100%")
  //       }
  //     }

  //       clicked5(){
  //         var size= document.getElementById('get5')
  //         if (size.getAttribute('width') == "100%"){
  //           size.setAttribute('width',"60%")
  //         }else{
  //           size.setAttribute('width',"100%")
  //         }
  //       }
      
  
}

