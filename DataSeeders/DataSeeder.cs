
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WeatherData.Models;

namespace WeatherData.DataSeeders { 

    public static class DataSeeder { 

        public static void Seed(
            DatabaseContext context, 
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager) { 
                
                // creare admin user
                if(!context.Users.Any()) 
                {
                    CreateAdminUser(context, roleManager, userManager)
                        .GetAwaiter().GetResult();  
                }

                if(!context.Addresses.Any()) 
                {
                    CreateAddress(context).GetAwaiter().GetResult(); 
                }
            }

        private static string RoleAdministrator = "Administrator";

        private static async Task CreateAdminUser(
            DatabaseContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager) { 

            // create admin role if doesn't exists
            if(!await roleManager.RoleExistsAsync(RoleAdministrator)) { 
                await roleManager.CreateAsync(new IdentityRole(RoleAdministrator));
            }

            // create admin user 
            var adminUser = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(), 
                UserName = "Admin", 
                Email = "admin@weatherdata.com"
            };

            // insert admin user to DB
            if(await userManager.FindByIdAsync(adminUser.Id) == null) 
            { 
                await userManager.CreateAsync(adminUser, "PaSSword12!");
                await userManager.AddToRoleAsync(adminUser, RoleAdministrator); 
                adminUser.EmailConfirmed = true; 
                adminUser.LockoutEnabled = false; 
            }

            await context.SaveChangesAsync();  
        }

         private static async Task CreateAddress(DatabaseContext context) { 

             Address address =  new Address() { 
                 AddressLine1 = "Mogilska",
                 AddressLine2 = "10/12",
                 ZipCode = "30-010",
                 City = "Kraków", 
                 Region = "małopolskie", 
                 Country = "Polska"
             };

             await context.Addresses.AddAsync(address); 
             await context.SaveChangesAsync(); 
         }
    }
}