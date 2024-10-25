using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieStoreApp.Models.Domain;

namespace MovieStoreApp.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUsers>
    {
       
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) 
        {

        }


        public DbSet<Genre> Genre { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
        public DbSet<Movie> Movie { get; set; }
    }

}
