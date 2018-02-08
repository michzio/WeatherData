
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeatherData.Models;
using WeatherData.Repositories;
using WeatherData.ViewModels;

namespace WeatherData.Controllers 
{
    public class WeatherDataController : BaseController
    {

        private IUnitOfWork _unitOfWork; 

        public WeatherDataController(
            DatabaseContext context, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration, 
            IUnitOfWork unitOfWork) : base(context, roleManager, userManager, configuration)
        {
            _unitOfWork = unitOfWork; 
        }

        // GET: /weather-datas/latest
        [HttpGet]
        public async Task<IActionResult> GetLatestWeatherData() 
        {
            try { 
                var weatherData = _unitOfWork.WeatherDatas.GetLatest(); 
                if(weatherData == null) { 
                    return NotFound(new { error = String.Format("Weather Data has not been found.")});
                }

                WeatherDataViewModel vmWeatherData = new WeatherDataViewModel(weatherData); 
                return Ok(vmWeatherData);

            } catch(Exception e) { 
                return BadRequest(new { error = e.Message }); 
            }
        }

        // GET: /weather-datas/{weatherDataId} 
        [HttpGet("{weatherDataId}")]
        public async Task<IActionResult> GetWeatherData(int weatherDataId) { 

            var weatherData = await _unitOfWork.WeatherDatas.GetAsync(weatherDataId); 
            if(weatherData == null) { 
                return NotFound(new { error = String.Format("Weather Data with id {0} has not been found.", weatherDataId) }); 
            }

            WeatherDataViewModel vmWeatherData = new WeatherDataViewModel(weatherData, false); 
            return Ok(vmWeatherData); 
        }


        // POST: /weather-datas
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateWeatherData([FromBody] WeatherDataViewModel vmWeatherData) 
        {

            if(vmWeatherData == null)
                return BadRequest(new { error = "No Weather Data object in request body."});
            if(!ModelState.IsValid)
                return BadRequest(ModelState); 

            try { 
               
               // create new WeatherData
               var newWeatherData = new WeatherData.Models.WeatherData
               {
                    Temperature = vmWeatherData.Temperature, 
                    Weather = vmWeatherData.Weather, 
                    Humidity = vmWeatherData.Humidity, 
                    WindSpeed = vmWeatherData.WindSpeed, 
                    PrecipitationProbability = vmWeatherData.PrecipitationProbability,
                    DateTime = (vmWeatherData.DateTime != null) ?  vmWeatherData.DateTime : DateTime.Now, 
                    AddressId = vmWeatherData.AddressId // if Address doesn't exists it throws error
               }; 

               // Add new WeatherData to DBContext and Save Changes 
               await _unitOfWork.WeatherDatas.AddAsync(newWeatherData); 
               _unitOfWork.Complete(); 

               vmWeatherData.WeatherDataId = newWeatherData.WeatherDataId; 
               return CreatedAtAction(nameof(GetWeatherData), new { weatherDataId = vmWeatherData.WeatherDataId }, vmWeatherData); 

            } catch(Exception e) 
            { 
                return BadRequest(new { error = e.Message }); 
            }

        }

    }
}