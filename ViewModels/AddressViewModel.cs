
using System.ComponentModel.DataAnnotations;
using WeatherData.Models;

namespace WeatherData.ViewModels {
     public class AddressViewModel { 

        public AddressViewModel() { }

        public AddressViewModel(Address address, bool includeReferences = true) { 

            // wrap Address repository object into AddressViewModel object 
            AddressId = address.AddressId; 
            AddressLine1 = address.AddressLine1; 
            AddressLine2 = address.AddressLine2; 
            ZipCode = address.ZipCode; 
            City = address.City; 
            Region = address.Region; 
            Country = address.Country; 
        }

        public int AddressId { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        [Required]
        public string AddressLine2 { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string Country { get; set; }

    }
}