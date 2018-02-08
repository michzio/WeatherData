import { Injectable } from "@angular/core";
import { WEB_API_URL } from "../shared/webapi.constants";
import { HttpClient } from "@angular/common/http";
import { IWeatherData } from "../models/weather-data.model";

export const WEATHER_DATA_WEB_API_URL = WEB_API_URL + "/weather-datas"; 

@Injectable() 
export class WeatherDataService { 
    
    private weatherDataUrl = WEATHER_DATA_WEB_API_URL; 

    constructor(private httpClient: HttpClient) { }

    createWeatherData(weatherData: IWeatherData) { 

        return this.httpClient.post<IWeatherData>(this.weatherDataUrl, weatherData);
    }

    getLatestWeatherData() { 
        
        return this.httpClient.get<IWeatherData>(this.weatherDataUrl + "/latest"); 
    }
}