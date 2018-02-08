import { WEB_API_URL } from "../shared/webapi.constants";
import { Injectable, Inject, PLATFORM_ID } from "@angular/core";
import { JwtHelperService } from "@auth0/angular-jwt"; 
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/operator/map'
import 'rxjs/add/operator/catch'
import { isPlatformBrowser } from "@angular/common";
import { IAuthObject } from "../models/auth-object.model";
import { IAccessToken } from "../models/access-token.model";
import { HttpHeaders } from "@angular/common/http";


export const AUTH_WEB_API_URL = WEB_API_URL + "/auth"; 

@Injectable() 
export class AuthService { 
    
    authKey: string = "auth"; 
    clientId: string = "WeatherData"

    // authentication endpoints 
    private authTokenUrl = AUTH_WEB_API_URL + "/token"; 
    private authLogoutUrl = AUTH_WEB_API_URL + "/logout"; 

    private jwtHelper = new JwtHelperService({}); 

    // store the URL to redirect after loggin in 
    public redirectUrl: string | null; 

    constructor(private http: HttpClient, 
                @Inject(PLATFORM_ID) private platformId: Object) { }

    login(usernameOrEmail: string, password: string) : Observable<any> { 
        
        var data = { 
            username: usernameOrEmail, 
            password: password, 
            client_id: this.clientId, 
            client_secret: "", // not implemented 
            grant_type: "password", 
            scope: "", // not implemented
        }

        return this.authTokenRequest(data); 
    }  
    

    logout() : Observable<boolean> {
        
        console.log("Logging out..."); 
        
        return this.http.post(this.authLogoutUrl, null)
            .map( (response) =>  { 
                // delete token from local storage
                this.setAuth(null); 
                return true 
            }).catch( (error) => {
                console.log("Error " + error);
                return Observable.throw(error);
            });
    }

    // persist authObject in localStorage 
    // or remove it if NULL argument is passed
    setAuth(authObject: IAuthObject | null) : void { 
        if(isPlatformBrowser(this.platformId)) { 
            if(authObject) { 
                // persist 
                localStorage.setItem(this.authKey, JSON.stringify(authObject)); 
            } else { 
                // remove 
                localStorage.removeItem(this.authKey); 
            }
        }
    }
    
    // retrieve authObject (or NULL) from localStorage 
    getAuth(): IAuthObject | null { 
        if(isPlatformBrowser(this.platformId)) { 
            var authItem = localStorage.getItem(this.authKey); 
            if(authItem) { 
                return JSON.parse(authItem); // authObject
            } else { 
                return null; 
            }
        }
        return null; 
    }


    // return true if the user is logged in 
    // otherwise false 
    isLoggedIn() : boolean { 
        var authObject = this.getAuth(); 
        if(authObject && authObject.access_token) { 
            const isAccessTokenValid : boolean = !this.jwtHelper.isTokenExpired(authObject.access_token);
            return isAccessTokenValid; 
        }
        return false; 
    }

    decodedAccessToken() : IAccessToken | null { 
        let authObject = this.getAuth(); 
        if(authObject && authObject.access_token) { 
            return this.jwtHelper.decodeToken(authObject.access_token); 
        }
        return null; 
    }

    // tires to refresh expired access token using refresh token 
    refreshToken() : Observable<boolean> { 
       
        var data = { 
            client_id: this.clientId, 
            grant_type: "refresh_token", 
            refresh_token: this.getAuth()!.refresh_token, 
            scope: "", // not implemented
        }

        return this.authTokenRequest(data); 
    }


    // request access & refresh tokens from the server 
    authTokenRequest(data: any) : Observable<any> { 

        // make authorization request 
        return this.http.post<IAuthObject>(
            this.authTokenUrl, // URL 
            data, // body 
            { 
                headers: new HttpHeaders() // headers
                    .set('Content-Type', 'application/json')
                    .set('Accept', 'application/json')
            })
            .map( (authObject) => { 

                console.log("Auth object received."); 
                console.log(authObject); 

                let access_token = authObject && authObject.access_token;
                if(access_token) { 
                    // successful login 
                    this.setAuth(authObject);
                    return true; 
                }

                // failed login
                return Observable.throw("Unauthorized"); 
            })
            .catch( (error) => { 
                return new Observable<any>(error); 
            });
    }
}