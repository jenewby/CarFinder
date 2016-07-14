using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.SqlClient;
using CarFinderNew.Areas.CFClass;
using CarFinderNew.Areas.Cars.Models;


namespace CarFinderNew.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public async Task<List<string>> GetPersons()
        {
            return await this.Database.SqlQuery<string>("Testing").ToListAsync();
        }

        public async Task<List<string>> GetYears() {
            return await this.Database.SqlQuery<string>("GetYears").ToListAsync();
        }

        public async Task<List<string>> GetMake(int year)
        {
            return await this.Database.SqlQuery<string>("GetMake @year",
                new SqlParameter("year", year)).ToListAsync();
        }

        public async Task<List<string>> GetTrim(int year, string make, string model)
        {
            return await this.Database.SqlQuery<string>("GetTrim @year, @make, @model", new SqlParameter("year", year), new SqlParameter("model", model), new SqlParameter("make", make)).ToListAsync();
        }

        public async Task<List<string>> GetModel(int year, string make)
        {
            return await this.Database.SqlQuery<string>("GetModel @year, @make", new SqlParameter("year", year), new SqlParameter("make", make)).ToListAsync();
        }

        public async Task<List<Cars>> GetCars(int year, string make, string model, string trim)
        {
            return await this.Database.SqlQuery<Cars>("GetCars @year, @make, @model, @trim", 
                new SqlParameter("year", year), 
                new SqlParameter("make", make ?? ""), 
                new SqlParameter("model", model ?? ""), 
                new SqlParameter("trim", trim ?? "")).ToListAsync();
        }

        public async Task<Cars> SingleCar(int year, string make, string model, string trim)
        {
            return await this.Database.SqlQuery<Cars>("GetCars @year, @make, @model, @trim",
                new SqlParameter("year", year),
                new SqlParameter("make", make ?? ""),
                new SqlParameter("model", model ?? ""),
                new SqlParameter("trim", trim ?? "")).FirstAsync();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
    }
}