import { Component, OnInit } from '@angular/core';
import { EmbedVideoService } from 'ngx-embed-video';
@Component({
  selector: 'app-youtube',
  templateUrl: './youtube.component.html',
  styleUrls: ['./youtube.component.css']
})
export class YoutubeComponent implements OnInit {

  constructor(private embedService: EmbedVideoService) 
  {
  }

  ngOnInit() {
  }

}
