import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from "@angular/forms"; 
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http'; 
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { AdminComponent } from './components/admin/admin.component';
import { LoginComponent } from './components/login/login.component'; 

// HTTP interceptors 
import { HTTP_INTERCEPTORS } from '@angular/common/http'; 
import { AuthInterceptor } from './shared/auth.interceptor';
import { AuthRefreshInterceptor } from './shared/auth-refresh.interceptor'; 
import { AuthService } from './services/auth.service';
import { AuthGuard } from './services/auth-guard.service';
import { AppRoutingModule } from './app-routing.module';
import { AddressService } from './services/address.service';
import { WeatherDataService } from './services/weather-data.service';
import { EnumMembersPipe } from './pipes/enum-members.pipe';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        AdminComponent,
        HomeComponent,
        LoginComponent, 

        EnumMembersPipe
    ],
    imports: [
        CommonModule,
        HttpModule,
        HttpClientModule, 
        FormsModule,
        ReactiveFormsModule,
        // Application Routing 
        AppRoutingModule,
    ], 
    providers: [
        AuthService, 
        {
            provide: HTTP_INTERCEPTORS, 
            useClass: AuthInterceptor, 
            multi: true
        }, 
        { 
            provide: HTTP_INTERCEPTORS, 
            useClass: AuthRefreshInterceptor, 
            multi: true
        }, 
        AuthGuard, 
        AddressService, 
        WeatherDataService, 

        EnumMembersPipe
    ]
})
export class AppModuleShared {
}
