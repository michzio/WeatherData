import { Component, OnInit } from "@angular/core";
import { FormGroup, Validators } from "@angular/forms";
import { AuthService } from "../../services/auth.service";
import { FormBuilder } from "@angular/forms";
import { Router } from "@angular/router";

@Component({
    selector: 'login', 
    templateUrl: 'login.component.html', 
    styleUrls: []
})
export class LoginComponent implements OnInit { 
   
    loginForm : FormGroup; 
    loginFailed : boolean = false; 

    constructor(private fb: FormBuilder, 
                private router: Router, 
                private authService: AuthService) { 
                    
                }

    ngOnInit(): void { 
        this.createForm(); 
    }
    
    createForm() { 
        this.loginForm = this.fb.group({
            email: ["", [Validators.required]], 
            password: ["", [Validators.required]]
        });
    }

    onSubmit(evt : any) { 
        evt.preventDefault(); 
        console.log(evt); 

        this.authService.login(this.email.value, this.password.value)
                .subscribe((success) => { 

                    // login successful 
                    this.loginFailed = false; 

                    // redirect user 
                    if(this.authService.redirectUrl) { 
                        this.router.navigate([this.authService.redirectUrl]); 
                        this.authService.redirectUrl = null; 
                    } else { 
                        this.router.navigate([""]); 
                    }
                }, (err) => { 
                    // login failure 
                    this.loginFailed = true; 
                    this.loginForm.setErrors({
                        "auth" : "Incorrect username or password"
                    });

                    //console.log(err); 
                }); 

    }

    get email() { return this.loginForm.get('email')!; }
    get password() { return this.loginForm.get('password')!; }
}