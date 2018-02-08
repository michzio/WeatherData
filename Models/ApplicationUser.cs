
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WeatherData.Models
{
    public class ApplicationUser : IdentityUser
    {
       // Navigation Properties
       public virtual List<RefreshToken> RefreshTokens { get; set; }
    }
}