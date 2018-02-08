import { WEB_API_URL } from "../shared/webapi.constants";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { IAddress } from "../models/address.model";


export const ADDRESS_WEB_API_URL = WEB_API_URL + "/addresses"; 

@Injectable() 
export class AddressService { 

    private addressUrl = ADDRESS_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }
    
    getAddresses() : Observable<[IAddress]> { 
        return this.httpClient.get<[IAddress]>(this.addressUrl); 
    }
}