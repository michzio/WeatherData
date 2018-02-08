import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Injectable } from "@angular/core";
import { CanActivateChild } from "@angular/router";
import { Observable } from "rxjs/Observable";
import { AuthService } from "./auth.service";


@Injectable() 
export class AuthGuard implements CanActivate, CanActivateChild { 
   
    constructor(private authService: AuthService, private router: Router) { }
   
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
       
        if(this.authService.isLoggedIn()) return true;
 
        // store the attempted URL for redirecting after successful logging in 
        this.authService.redirectUrl = state.url
 
        // navigate to the login page with extra
        this.router.navigate(['/login']); 
        return false; 
    }

    canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Promise<boolean> | Observable<boolean> {
        return this.canActivate(childRoute, state); 
    }
}