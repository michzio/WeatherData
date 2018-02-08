import { Injectable, Injector } from "@angular/core";
import { HttpInterceptor } from "@angular/common/http";
import { HttpRequest } from "@angular/common/http";
import { HttpHandler } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { HttpSentEvent } from "@angular/common/http";
import { HttpHeaderResponse } from "@angular/common/http";
import { HttpProgressEvent } from "@angular/common/http";
import { HttpResponse } from "@angular/common/http";
import { HttpUserEvent } from "@angular/common/http";
import { AuthService } from "../services/auth.service";
import { HttpEvent } from "@angular/common/http";


@Injectable() 
export class AuthInterceptor implements HttpInterceptor { 

    constructor(private injector: Injector) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
       
        // inject Authentication Service manually 
        let authService = this.injector.get(AuthService); 

        // get authObject from the service 
        let authObject = authService.getAuth(); 

        // clone the request to add the new header 
        let authRequest = request.clone({
            setHeaders: { 
                Authorization: `Bearer ${authObject? authObject.access_token : ""}`
            }
        });
        
        // pass on the cloned request 
        // instead of the original one
        return next.handle(authRequest);
    }

}