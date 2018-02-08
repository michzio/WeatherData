import { Component } from '@angular/core';
import { AddressService } from '../../services/address.service';
import { FormGroup } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { IAddress } from '../../models/address.model';

@Component({
    selector: 'admin',
    templateUrl: './admin.component.html'
})
export class AdminComponent {

    weatherForm: FormGroup; 
    updateFailed = false; 

    addresses : Observable<[IAddress]>; 

    constructor(
            private fb: FormBuilder, 
            private addressService: AddressService) { 

            this.createForm();
            this.loadAddresses(); 

    }

    createForm() { 

        this.weatherForm = this.fb.group({
            address: ["", [Validators.required]]
        });
    }

    loadAddresses() { 

        this.addresses = this.addressService.getAddresses(); 
    }


}
