import { IAddress } from "./address.model";
import { WeatherType } from "./enums/weather-type.enum";

export interface IWeatherData {
    weatherDataId? : number; 
    temperature : number; 
    weather : WeatherType;  
    dateTime? : Date; 
    humidity : number; 
    windSpeed : number
    precipitationProbability : number
    addressId? : number
    
    address? : IAddress; 
}