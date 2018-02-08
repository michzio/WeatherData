import { Injectable } from "@angular/core";
import { WEB_API_URL } from "../shared/webapi.constants";
import { HttpClient } from "@angular/common/http";

export const WEATHER_DATA_WEB_API_URL = WEB_API_URL + "/weather-datas"; 

@Injectable() 
export class WeatherDataService { 
    
    private weatherDataUrl = WEATHER_DATA_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    
}