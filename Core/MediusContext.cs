using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class MediusContext : DbContext
    {
        public MediusContext()
        {

        }
        public MediusContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<IpFilter> IpFilters { get; set; }
        public DbSet<UsersIp> UsersIps { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if ("production".Equals(environment, StringComparison.OrdinalIgnoreCase))
            {
                // production environment
                //optionsBuilder.UseSqlServer("Server=tcp:skeddle.database.windows.net,1433;Initial Catalog=skeddle;Persist Security Info=False;User ID=appadmin;Password=qvoA4gHL;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", options => options.EnableRetryOnFailure());
            }
            else
            {
                // test environment
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-8KCKC9Q\\SQLEXPRESS;Initial Catalog=MediusDB;Trusted_Connection=True;Connection Timeout=30;", options => options.EnableRetryOnFailure());
            }
        }
    }
}
