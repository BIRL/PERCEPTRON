import { Http } from '@angular/http';
import { DomSanitizer } from '@angular/platform-browser';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
export declare class EmbedVideoService {
    private http;
    private sanitizer;
    private validYouTubeOptions;
    private validVimeoOptions;
    private validDailyMotionOptions;
    constructor(http: Http, sanitizer: DomSanitizer);
    embed(url: any, options?: any): any;
    embed_youtube(id: string, options?: any): string;
    embed_vimeo(id: string, options?: any): string;
    embed_dailymotion(id: string, options?: any): string;
    embed_image(url: any, options?: any): any;
    private embed_youtube_image(id, options?);
    private embed_vimeo_image(id, options?);
    private embed_dailymotion_image(id, options?);
    private parseOptions(options);
    private serializeQuery(query);
    private sanitize_iframe(iframe);
    private detectVimeo(url);
    private detectYoutube(url);
    private detectDailymotion(url);
}
