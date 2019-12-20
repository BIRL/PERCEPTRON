import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms'
import { Http } from '@angular/http';

@Component({
  selector: 'app-testform',
   templateUrl: './testform.component.html',
  styleUrls: ['./testform.component.css']
})
export class TestformComponent implements OnInit {

    form: FormGroup;

    constructor(private fbuilder: FormBuilder,
                private http: Http) { }

    ngOnInit(){
    }

    onSubmit(form: any): void {
      console.log('you submitted value:', form);
    }
  
}
