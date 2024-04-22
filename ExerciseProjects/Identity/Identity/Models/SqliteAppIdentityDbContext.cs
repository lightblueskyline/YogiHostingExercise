using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Identity.Models
{
    public class SqliteAppIdentityDbContext : IdentityDbContext<AppUser>
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public SqliteAppIdentityDbContext(IWebHostEnvironment environment)
        {
            this.webHostEnvironment = environment;
        }

        // https://benfoster.io/blog/aspnet-core-multi-tenancy-data-isolation-with-entity-framework
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=IdentityTest.db");
        }
    }
}
