
using System;
using System.ComponentModel.DataAnnotations;
using WeatherData.Models;
using static WeatherData.Models.WeatherData;

namespace WeatherData.ViewModels {

    public class WeatherDataViewModel { 

        public WeatherDataViewModel() { }

        public WeatherDataViewModel(WeatherData.Models.WeatherData weatherData, bool includeReferences = true) { 

            // wrap WeatherData repository object into WeatherDataViewModel object 
            WeatherDataId = weatherData.WeatherDataId;
            Temperature = weatherData.Temperature; 
            Weather = weatherData.Weather; 
            DateTime = weatherData.DateTime; 
            Humidity = weatherData.Humidity; 
            WindSpeed = weatherData.WindSpeed; 
            PrecipitationProbability = weatherData.PrecipitationProbability;
            AddressId = weatherData.AddressId; 
            if(includeReferences && weatherData.Address != null) { 
                Address = new AddressViewModel(weatherData.Address);
            }
        }

        public int WeatherDataId { get; set; }
        [Required]
        public int Temperature { get; set; }
        [Required]
        public WeatherType Weather { get; set; }
        public DateTime DateTime { get; set; }
        [Required]
        public int Humidity { get; set; }
        [Required]
        public int WindSpeed { get; set; }
        [Required]
        public int PrecipitationProbability { get; set; }

        // FK properties 
        [Required]
        public int AddressId { get; set; }

        // Navigation properties 
        public AddressViewModel Address { get; set; }
    }

}