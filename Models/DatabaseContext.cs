
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WeatherData.Models
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser> 
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        #region Properties 
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<WeatherData> WeatherDatas { get; set; }
        public DbSet<Address> Addresses { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<RefreshToken>().Property(rt => rt.RefreshTokenId).ValueGeneratedOnAdd(); 
            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(au => au.RefreshTokens) 
                .HasForeignKey(rt => rt.UserId)
                .HasPrincipalKey(au => au.Id);

            modelBuilder.Entity<WeatherData>()
                .HasOne(wd => wd.Address)
                .WithMany(a => a.WeatherDatas)
                .HasForeignKey(wd => wd.AddressId)
                .OnDelete(DeleteBehavior.Cascade); 

        }        
    }
}