using Microsoft.EntityFrameworkCore;
using MinimalApis.Demo.Context.Models.Movies;
using MinimalApis.Demo.Context.Models.Users;

namespace MinimalApis.Demo.Context
{
    public class MinimalApisContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("MinimalApisDb");
        }
    }
}