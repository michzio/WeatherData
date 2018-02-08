import { Component } from '@angular/core';
import { WeatherType } from '../../models/enums/weather-type.enum';
import { WeatherDataService } from '../../services/weather-data.service';
import { IWeatherData } from '../../models/weather-data.model';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {

    public weatherTypes = WeatherType; // exporting WeatherType enum to use in template
    weatherData : IWeatherData | null; 

    constructor(private weatherDataService: WeatherDataService) {

        this.loadWeatherData()
     }

     private loadWeatherData() { 
        this.weatherDataService.getLatestWeatherData()
            .subscribe( (weatherData : IWeatherData) => { 
                this.weatherData = weatherData; 
                console.log(weatherData); 
            }, (err) => { 
                this.weatherData = null; 
            }); 
     }
}
