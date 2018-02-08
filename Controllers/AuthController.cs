using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WeatherData.Models;
using WeatherData.ViewModels;

namespace WeatherData.Controllers { 

    [Route("auth")]
    public class AuthController : BaseController { 

        private SignInManager<ApplicationUser> SignInManager; 

        public AuthController(
            DatabaseContext context, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration) 
            : base(context, roleManager, userManager, configuration)
        { 
            SignInManager = signInManager; 
        }

        [HttpPost("token")]
        public async Task<IActionResult> Auth([FromBody] TokenRequestViewModel tokenRequestViewModel) { 

            if(!ModelState.IsValid) 
                return BadRequest(ModelState); 

            switch(tokenRequestViewModel.grant_type) 
            { 
                case "password": 
                    return await GetNewToken(tokenRequestViewModel); 
                case "refresh_token":
                    return await RefreshToken(tokenRequestViewModel); 
                default: 
                    // not supported - return a HTTP 401 (Unauthorized)
                    return new UnauthorizedResult(); 
            }
        }

       [HttpPost("logout")]
       public IActionResult Logout() { 
           
           if(HttpContext.User.Identity.IsAuthenticated)
           {
               SignInManager.SignOutAsync().Wait(); 
           }

           return Ok(new { logout = "Success" }); 
       }

        private async Task<IActionResult> GetNewToken(TokenRequestViewModel tokenRequestViewModel) { 

            try { 
                // find user by username or email address 
                var user = await UserManager.FindByNameAsync(tokenRequestViewModel.username);
                if(user == null && tokenRequestViewModel.username.Contains("@")) { 
                    user = await UserManager.FindByEmailAsync(tokenRequestViewModel.username); 
                }

                if(user == null) { 
                    // user doesn't exist
                    return NotFound(); 
                }

                if(!await UserManager.CheckPasswordAsync(user, tokenRequestViewModel.password)) { 
                    // password mismatch 
                    return Unauthorized(); 
                }

                // create authentication token 
                // 1. create refresh token 
                var refreshToken = CreateRefreshToken(tokenRequestViewModel.client_id, user.Id); 
                // 2. store refresh token in DB 
                DatabaseContext.Add(refreshToken); 
                DatabaseContext.SaveChanges(); 
                // 3. create access token 
                var accessToken = CreateAccessToken(user.Id);
                var tokenExpirationMins = Configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");

                return Ok(new TokenResponseViewModel()
                {
                    access_token = accessToken,
                    expiration = tokenExpirationMins,
                    refresh_token = refreshToken.TokenValue
                }); 

            } catch(Exception ex)
            {
                return new UnauthorizedResult(); 
            }
        }

        private async Task<IActionResult> RefreshToken(TokenRequestViewModel tokenRequestViewModel) { 

            try { 

                // check existance of refresh token in DB
                var refreshToken = await DatabaseContext.RefreshTokens 
                    .FirstOrDefaultAsync(t => 
                        t.ClientId == tokenRequestViewModel.client_id &&
                        t.TokenValue == tokenRequestViewModel.refresh_token );

                if(refreshToken == null)
                    return NotFound(); 

                // check user existance for refresh token 
                var user = await UserManager.FindByIdAsync(refreshToken.UserId);
                if(user == null)
                    return NotFound();  

                // generate new refresh token 
                var newRefreshToken = CreateRefreshToken(refreshToken.ClientId, refreshToken.UserId); 

                // remove old one 
                DatabaseContext.RefreshTokens.Remove(refreshToken);
                // add new refresh token 
                DatabaseContext.RefreshTokens.Add(refreshToken); 

                DatabaseContext.SaveChanges(); 

                // create new access token 
                var accessToken = CreateAccessToken(refreshToken.UserId);
                var tokenExpirationMins = Configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");


                return Ok(new TokenResponseViewModel()
                {
                    access_token = accessToken,
                    expiration = tokenExpirationMins,
                    refresh_token = refreshToken.TokenValue
                }); 

                return null; 
            } catch(Exception ex) { 
                return Unauthorized(); 
            }

        }

        private RefreshToken CreateRefreshToken(string clientId, string userId) { 

            return new RefreshToken() 
            {
                ClientId = clientId, 
                TokenValue = Guid.NewGuid().ToString("N"), 
                CreatedDate = DateTime.UtcNow, 
                UserId = userId 
            }; 
        }

        private string CreateAccessToken(string userId)
        { 
            DateTime now = DateTime.UtcNow; 

            // claims for JWT token
            var claims = new[] { 
                new Claim(JwtRegisteredClaimNames.Sub, userId), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString())
                // more claims ... 
            };

            var tokenExpirationMins = Configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:SecretKey"])); 
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256); 

            var token = new JwtSecurityToken(
                issuer: Configuration["Auth:Jwt:Issuer"],
                audience: Configuration["Auth:Jwt:Audience"], 
                claims: claims, 
                notBefore: now, 
                expires: now.Add(TimeSpan.FromMinutes(tokenExpirationMins)), 
                signingCredentials: signingCredentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
 
            return accessToken; 
        }
        
    }
}