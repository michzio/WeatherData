using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeatherData.Models;
using WeatherData.Repositories;
using WeatherData.ViewModels;

namespace WeatherData.Controllers { 

    [Route("addresses")]
    public class AddressesController : BaseController { 

        private IUnitOfWork _unitOfWork;

        public AddressesController(
            DatabaseContext context, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration, 
            IUnitOfWork unitOfWork) : base(context, roleManager, userManager, configuration)
        {
            _unitOfWork = unitOfWork; 
        }

        // GET: /addresses/{addressId}
        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddress(int addressId) { 
            
            Address address = await _unitOfWork.Addresses.GetAsync(addressId);
            if(address == null) { 
                return NotFound(new { error = String.Format("Address with id {0} has not been found.", addressId) });
            }

            AddressViewModel vmAddress = new AddressViewModel(address); 
            return Ok(vmAddress); 
        }

        // GET: /addresses
        [HttpGet]
        public async Task<IActionResult> GetAddresses() { 
            
            List<AddressViewModel> vmAddresses = (await _unitOfWork.Addresses.GetAllAsync())
                                .Select(a => new AddressViewModel(a, false)).ToList(); 

            return Ok(vmAddresses);
        }
    }
}