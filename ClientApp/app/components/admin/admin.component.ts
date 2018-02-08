import { Component } from '@angular/core';
import { AddressService } from '../../services/address.service';
import { FormGroup } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { IAddress } from '../../models/address.model';
import { WeatherType } from '../../models/enums/weather-type.enum';
import { IWeatherData } from '../../models/weather-data.model';
import { WeatherDataService } from '../../services/weather-data.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.css']
})
export class AdminComponent {

    public weatherTypes = WeatherType; // exporting WeatherType enum to use in template

    weatherForm: FormGroup; 
    updateFailed = false; 
    updateMessage: string | null = null; 

    addresses : Observable<[IAddress]>; 

    constructor(
            private fb: FormBuilder, 
            private addressService: AddressService, 
            private weatherDataService: WeatherDataService, 
            private authService: AuthService, 
            private router: Router) { 

            this.createForm();
            this.loadAddresses(); 

    }

    createForm() { 

        this.weatherForm = this.fb.group({
            address: ["", [Validators.required]], 
            temperature: ["", [Validators.required, 
                               Validators.min(-50), 
                               Validators.max(200)]], 
            weatherType: ["", [Validators.required]],
            humidity: ["", [Validators.required, 
                            Validators.min(0), 
                            Validators.max(100)]], 
            windSpeed: ["", [Validators.required]], 
            precipitationProbability: ["", [Validators.required, 
                                            Validators.min(0), 
                                            Validators.max(100)]]
        });
    }

    loadAddresses() { 

        this.addresses = this.addressService.getAddresses(); 
    }

    onSubmit(evt : any) { 
        evt.preventDefault(); 
        console.log(evt);

        if(!this.weatherForm.valid) { 
            this.updateFailed = true; 
            this.weatherForm.setErrors({
                "update" : "Weather data form invalid." 
            });
            return; 
        }

        // build a temporary weather data object from form values 
        var tempWeatherData = <IWeatherData>{}; 
        tempWeatherData.temperature = this.temperature.value; 
        tempWeatherData.weather = this.weatherType.value; 
        tempWeatherData.humidity = this.humidity.value; 
        tempWeatherData.windSpeed = this.windSpeed.value; 
        tempWeatherData.precipitationProbability = this.precipitationProbability.value;
        tempWeatherData.addressId = this.address.value;

        this.weatherDataService.createWeatherData(tempWeatherData) 
                        .subscribe((weatherData) => { 
                            console.log("New weather inserted with id: " + weatherData.weatherDataId); 
                            this.updateFailed = false; 
                            this.updateMessage = "Weather Data inserted successfully."; 
                            this.resetForm(); 
                        }, (error) => { 

                        })
    }

    private resetForm() { 
        this.createForm(); 
    }

    logout(evt : any) { 
        evt.preventDefault(); 
        console.log("Logout");
        this.authService.logout().subscribe((success) => { 
            this.router.navigate([""]);
        }); 
    }

    get address() { return this.weatherForm.get('address')!; }
    get temperature() { return this.weatherForm.get('temperature')!; }
    get weatherType() { return this.weatherForm.get('weatherType')!; }
    get humidity() { return this.weatherForm.get('humidity')!; }
    get windSpeed() { return this.weatherForm.get('windSpeed')!; }
    get precipitationProbability() {  return this.weatherForm.get('precipitationProbability')!; } 
}
