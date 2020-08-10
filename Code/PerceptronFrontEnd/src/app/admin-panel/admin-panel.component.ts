import { Component, OnInit } from '@angular/core';
import { ConfigService } from '../config.service';
import { FormControl } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';


@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {

  constructor(private route: ActivatedRoute, private router: Router, private _httpService: ConfigService) { }

  ngOnInit() {
  }


  onSubmit(file: any): void {
  
  }


  // let fi = this.imgFileInput.nativeElement;
  //   let stats: any = 'false';
  //   console.log(form);
  //   stats = this._httpService.postJSON(form, fi.files);

}
