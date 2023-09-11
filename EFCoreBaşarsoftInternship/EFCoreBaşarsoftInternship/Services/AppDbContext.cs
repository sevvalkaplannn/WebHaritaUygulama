using EFCoreBaşarsoftInternship.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EFCoreBaşarsoftInternship
{
    public class AppDbContext : DbContext
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Door> doors { get; set; }

    }

}
