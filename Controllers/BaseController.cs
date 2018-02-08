
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeatherData.Models;

namespace WeatherData.Controllers { 

    public class BaseController : Controller { 

        public BaseController(
            DatabaseContext context, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            IConfiguration configuration
        ) { 
            DatabaseContext = context; 
            RoleManager = roleManager; 
            UserManager = userManager; 
            Configuration = configuration; 
        }

        protected DatabaseContext DatabaseContext; 
        protected RoleManager<IdentityRole> RoleManager; 
        protected UserManager<ApplicationUser> UserManager; 
        protected IConfiguration Configuration; 
    }
}