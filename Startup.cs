using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WeatherData.Models;

namespace WeatherData
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add EntityFramework support for SqlServer 
            services.AddEntityFrameworkSqlServer(); 

            // Add Identity Services & Stores 
            services.AddIdentity<ApplicationUser, IdentityRole>(options => { 
                options.SignIn.RequireConfirmedEmail = false; 
                options.User.RequireUniqueEmail = true; 
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true; 
                options.Password.RequiredLength = 7;
            })
            .AddEntityFrameworkStores<DatabaseContext>() 
            .AddDefaultTokenProviders(); 

            // Add Authentication
            services.AddAuthentication(options => {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => { 
                options.RequireHttpsMetadata = false; 
                options.SaveToken = true; 
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
                    // the signing key must match
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:SecretKey"])),
                    ValidateIssuerSigningKey = true,
                
                    // validate the JWT Issuer (iss) claim 
                    ValidIssuer = Configuration["Auth:Jwt:Issuer"],
                    ValidateIssuer = true, 

                    // validate the JWT Audience (aud) claim 
                    ValidAudience = Configuration["Auth:Jwt:Audience"], 
                    ValidateAudience = true, 

                    // validate the token expiration time 
                    ValidateLifetime = true, 
                    RequireExpirationTime = true, 

                    // if you want to allow a certain amount of clock drift, set that here: 
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add Database Context 
            // Use Azure SQL Database if Azure, otherwise local MS SQL 
            if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production") { 
                services.AddDbContext<DatabaseContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("AzureSQLServerDatabase"))
                );
            } else { 
                services.AddDbContext<DatabaseContext>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("MacOsSQLServerDatabase"))
                );
            }

            // Automatically perform database migration (useful when on Azure) 
            services.BuildServiceProvider().GetService<DatabaseContext>().Database.Migrate(); 

            services.AddMvc()
                    .AddXmlDataContractSerializerFormatters() 
                    .AddJsonOptions(options => { 
                        options.SerializerSettings.ReferenceLoopHandling = 
                                                    Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
           
           // registration of custom objects for dependency injection 
           //services.AddScoped<IUnitOfWork, UnitOfWork>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug(); 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // add a custom Jwt Provider to generate Tokens 
          /*  app.UseJwtProvider(new JwtProviderOptions() { 
                Path = Configuration["Auth:Jwt:TokenEndPoint"], 
                Issuer = Configuration["Auth:Jwt:Issuer"], 
                Audience = Configuration["Auth:Jwt:Audience"], 
                TokenExpiration = TimeSpan.FromMinutes(int.Parse(Configuration["Auth:Jwt:TokenExpiration"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:SecretKey"])), 
                    SecurityAlgorithms.HmacSha256)
            }); */ 

            // add the Jwt Bearer Authentication to validate Tokens 
            app.UseAuthentication(); 

            // add MVC to the pipeline 
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
