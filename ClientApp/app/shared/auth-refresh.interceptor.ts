import { HttpInterceptor } from "@angular/common/http";
import { Injectable, Injector } from "@angular/core";
import { HttpRequest } from "@angular/common/http";
import { AuthService } from "../services/auth.service";
import { HttpHandler } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { HttpEvent } from "@angular/common/http";
import { Router } from "@angular/router";
import { HttpResponse, HttpErrorResponse } from "@angular/common/http";
import 'rxjs/add/operator/do'
import { ObservableInput } from "rxjs/Observable";
import { HttpClient } from "@angular/common/http";


// it will capture 401 - Http Unauthorized errors 
// and try to refresh access token using refresh token 
@Injectable() 
export class AuthRefreshInterceptor implements HttpInterceptor { 
  
   
    
    currentRequest: HttpRequest<any>; 
    authService : AuthService; 

    constructor(private injector: Injector, 
                private router : Router) { }


    
    intercept(request: HttpRequest<any>, next: HttpHandler) : Observable<HttpEvent<any>> {
        
        // inject Authentication Service manually
        this.authService = this.injector.get(AuthService); 
        let authObject = this.authService.getAuth(); 

        // request with access token 
        if(authObject && authObject.access_token) { 

            // save current request 
            this.currentRequest = request; 

            return next.handle(request)
                       .do((event: HttpEvent<any>) => { 
                            if(event instanceof HttpResponse) { 
                                //  do nothing
                            }
                        })
                        .catch( (error) => { 
                            return this.handleError(error)
                        })

        } else { 
            return next.handle(request);
        }
    }

    handleError(err: any) : Observable<HttpEvent<any>> { 


        if( err instanceof HttpErrorResponse) { 
            if( err.status === 401) { 
                // 401 - Http Unauthorized Error 
                // Access Token might be expired
                // Try to get a new one using refresh token 
                console.log("Access token has expired. Attempting to refresh it..."); 

                this.authService.refreshToken()
                    .subscribe( (success) => { 
                        if(success) { 
                            console.log("access token refreshed successfully"); 

                            // re-submit the failed request 
                            let httpClient = this.injector.get(HttpClient); 
                            httpClient.request(this.currentRequest).subscribe( 
                                (res: any) => { },
                                (error : any) => { console.log(error);  });
                        } else { 
                            console.log("access token refresh failed");

                            // erase expired token 
                            this.authService.logout(); 

                            // redirect to login page 
                            this.router.navigate(["login"]); 
                        }
                    }, (error) => { 
                        console.log(error)
                    });
            }
        }

        return Observable.throw(err); 
    }
}