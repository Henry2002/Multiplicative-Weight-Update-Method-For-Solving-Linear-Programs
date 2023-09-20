import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { BaseResult } from '../models/baseresult';




@Injectable({
    providedIn: 'root',
})

//The web service deals with the base results and then communicates with the error-report component so that the error message can be displayed//
export class WebService {
    

  constructor(private http: HttpClient, private router: Router) { }

    public get<T>(url: string, callback: Function) {


      return this.http.get<BaseResult<T>>(url, { }).subscribe(result => {
        if (result.success) {
          if (result.result != null)
            callback(result.result);
          else
            callback(result);
        }
        else {
          //Communicate with the error report components
          callback(result.errorMessage);
        }
        (err: HttpErrorResponse) => {
            console.log(err.error);
            console.log(err.name);
            console.log(err.status);
            console.log(err.message);
          }
        });
        
    }

    public post<T>(url: string, model: object, callback: Function) {

      return this.http.post<BaseResult<T>>(url, model, {}
        ).subscribe(result => {
            if (result.success) {
                if (result.result != null)
                    callback(result.result, true);
                else
                    callback(result, true);
            }
            else {
              //Communicate with the error report components
              callback(result.errorMessage, false);
            }
        }), (err: HttpErrorResponse) => {
                console.log(err.error);
                console.log(err.name);
                console.log(err.status);
                console.log(err.message);
            }
    };
 


    }

