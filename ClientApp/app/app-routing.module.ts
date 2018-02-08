import { NgModule } from '@angular/core'; 
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { AdminComponent } from './components/admin/admin.component';
import { AuthGuard } from './services/auth-guard.service';
import { LoginComponent } from './components/login/login.component';

const appRoutes : Routes = [
    { 
        path: '', 
        redirectTo: 'home', 
        pathMatch: 'full' 
    },
    { 
        path: 'home', 
        component: HomeComponent 
    },
    { 
        path: 'admin', 
        component: AdminComponent, 
        canActivate: [AuthGuard], 
    },
    {
        path: 'login', 
        component: LoginComponent
    }, 
    { 
        path: '**', 
        redirectTo: 'home' 
    }
]

@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes, /* { preloadingStrategy: SelectivePreloadingStrategy } */)
    ], 
    exports: [
        RouterModule
    ], 
    providers: [ 

    ]
})
export class AppRoutingModule { }