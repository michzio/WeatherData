
using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherData.Models { 

    public class WeatherData 
    {
        public enum WeatherType
        {
            Clear,
            MostlySunny, 
            PartlyCloudy,
            MostlyCloudy, 
            Cloudy, 
            Overcast
        };

        [Key]
        public int WeatherDataId { get; set; }
        [Required]
        public int Temperature { get; set; }
        [Required]
        public WeatherType Weather { get; set; }
        [Required]
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
        public Address Address { get; set; }
    }
}