import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Http, ResponseContentType } from '@angular/http';
import 'rxjs/add/operator/map';
import { Headers } from '@angular/http';

import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { database } from 'firebase/app';
import * as firebase from 'firebase/app';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';


@Injectable()
export class ConfigService {
    resultant: any;
    baseApiUrl = "http://localhost:52340/";
    
    

    constructor(private _http: Http) { }

    getJSON() {
        return this._http.get('http://date.jsontest.com')
            .map(res => res.json())
    }

    postbugform(form) {
        let formData: FormData = new FormData();
        var json = JSON.stringify(form);

        formData.append('Jsonfile', json);
        console.log(json);

        let headers = new Headers();
        headers.append('Accept', 'application/json');
        return this._http.post(this.baseApiUrl + '/api/search/bug_form', formData, { headers: headers })
            .map(res => res.json())
            .subscribe(
                data => console.log('success'),
                error => console.log(error)
            )
    }

    stuff() {
        let json = "ok";
        let headers = new Headers();
        // alert(json);
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/stat', '=' + json, { headers: headers })

            .map(res => res.json())
    }

    postJSON(form, file) {

        let formData: FormData = new FormData();

        var json = JSON.stringify(form);

        formData.append('Jsonfile', json);
        for (let i = 0; i < file.length; i++) {
            formData.append('uploadFile', file[i], file[i].name);
        }

        console.log(json);
        let headers = new Headers();
        headers.append('Accept', 'application/json');
        return this._http.post(this.baseApiUrl + '/api/search/File_upload', formData, { headers: headers })
            .map(res => res.json())
            .subscribe(
                data => console.log('success'),
                error => console.log(error)
            )
    }

    fdrform(form, file){
        let formData: FormData = new FormData();

        var json = JSON.stringify(form);

        formData.append('Jsonfile', json);
        for (let i = 0; i < file.length; i++) {
            formData.append('uploadFile', file[i], file[i].name);
        }

        console.log(json);
        let headers = new Headers();
        headers.append('Accept', 'application/json');
        return this._http.post(this.baseApiUrl + '/api/search/FDR_Data_upload', formData, { headers: headers })
            .map(res => res.json())
            // .subscribe(
            //     data => console.log('success'),
            //     error => console.log(error)
            // )
    }

    postpattern(form) {


        let formData: FormData = new FormData();
        var json = JSON.stringify(form);

        formData.append('Jsonfile', json);

        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/Tpg', '=' + json, { headers: headers })

            .map(res => res.json())


    }
    GetPattern(qid) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/Post_pattern', '=' + qid, { headers: headers })
            .map(res => {
                return res.json()
            });
    }

    GetScanReslts(qid) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/Post_scan_results', '=' + qid, { headers: headers })
            .map(res => {
                return res.json()
            });
    }

    GetResultsDownload(qid): Observable<any> {

        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/ResultsDownload', '=' + qid, { headers: headers })
            .map(res => {
                return res.json()
            });
    }


    postDatabase(form, file) {

        let formData: FormData = new FormData();

        var json = JSON.stringify(form);

        formData.append('Jsonfile', json);

        for (let i = 0; i < file.length; i++) {
            formData.append('uploadFile', file[i], file[i].name);
        }
        console.log(json);
        let headers = new Headers();
        headers.append('Accept', 'application/json');
        return this._http.post(this.baseApiUrl + '/api/search/DatabaseUpdate', formData, { headers: headers })
            .map(res => res.json())
            .subscribe(
                data => console.log('success'),
                error => console.log(error)
            )
    }

    GetDatabaseDownload(NameOfDataBase): Observable<any> {

        // let formData: FormData = new FormData();

        // var json = JSON.stringify(form);

        // formData.append('Jsonfile', json);

        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/DatabaseDownload', '=' + NameOfDataBase, { headers: headers })
            .map(res => {
                return res.json()
            });
    }



    GetScReslts(qid) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/Post_sc_results', '=' + qid, { headers: headers })
            .map(res => {
                return res.json()
            });
    }

    GetXicReslts(qid) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/Post_xic_results', '=' + qid, { headers: headers })
            .map(res => {
                return res.json()
            });
    }


    GetRegReslts(qid) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/Post_reg_results', '=' + qid, { headers: headers })
            .map(res => {
                return res.json()
            });
    }

    getUserHistory(userId) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        if (userId.emailVerified) {
            return this._http.post(this.baseApiUrl + '/api/search/Post_history', '=' + userId.email, { headers: headers })
                .map(res => {
                    return res.json()
                });
        }
        else { //For Guest's Search Results & History
            return this._http.post(this.baseApiUrl + '/api/search/Post_history', '=' + userId.uid, { headers: headers }).map(res => {
                return res.json()
            });
        }
    }

    GetSummaryReslts(qid, fileId) {
        let str = qid + "," + fileId;
        console.log(str);
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/post_summary_results', '=' + str, { headers: headers })
            .map(res => {
                return res.json()
            });
    }
    
    getfile(form) {

       

        var json = JSON.stringify(form);
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/data', '=' + json, { headers: headers })
            .map(res => res.json())
            .subscribe(
                data => console.log('success'),
                error => console.log(error)
            )
    }

    GetDetailedReslts(qid, resId) {
        let QueryIdResultId = qid + "," + resId;
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/Post_detailed_results', '=' + QueryIdResultId, { headers: headers })
            .map(res => {
                return res.json()
            });
    }

    GetDetailedProteinHitViewResults(resId) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
        return this._http.post(this.baseApiUrl + '/api/search/Post_DetailedProteinHitView_results', '=' + resId, { headers: headers })
            .map(res => {
                return res.json()
            });
    }
    // CreateDetailedProteinViewHit(resId) {
    //     let headers = new Headers();
    //     headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');
    //     return this._http.post(this.baseApiUrl + '/api/search/Post_detailed_results/Create_Detailed_Protein_View_Hit', '=' + resId, { headers: headers })
    //         .map(res => {
    //             return res.json()
    //         });
    // }

    // postFile(form, file) {

    //     let formData: FormData = new FormData();

    //     var json = JSON.stringify(form);

    //     formData.append('Jsonfile', json);
    //     for (let i = 0; i < file.length; i++) {
    //         formData.append('uploadFile', file[i], file[i].name);
    //     }

    //     console.log(json);
    //     let headers = new Headers();
    //     headers.append('Accept', 'application/json');
    //     return this._http.post(this.baseApiUrl + '/api/search/FASTA_File_upload', formData, { headers: headers })
    //         .map(res => res.json())
    //         .subscribe(
    //             data => console.log('success'),
    //             error => console.log(error)
    //         )
    // }


}

export interface SearchItem {
    FileName: string;
    ProteinId: string;
    Score: string;
    molW: string;
    Truncation: string;
    Frags: string;
    Mods: string;
}
