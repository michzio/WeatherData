
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherData.Models { 

    public class Address { 
        
        [Key] 
        public int AddressId { get; set; }
        [Required]
        [MaxLength(150)]
        public string AddressLine1 { get; set; }
        [Required]
        [MaxLength(150)]
        public string AddressLine2 { get; set; }
        [Required]
        [MaxLength(6)]
        [MinLength(6)]
        public string ZipCode { get; set; }
        [Required]
        [MaxLength(150)]
        public string City { get; set; }
        [Required]
        [MaxLength(150)]
        public string Region { get; set; }
        [Required]
        [MaxLength(150)]
        public string Country { get; set; }

        // Navigation properties 
        public List<WeatherData> WeatherDatas { get; set; }
    }
}