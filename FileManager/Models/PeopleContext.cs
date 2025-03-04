using Microsoft.EntityFrameworkCore;
using FileManagerLibrary.Models;

namespace FileManagerAPI.Models
{
    public class PeopleContext : DbContext
    {
        public DbSet<Person> People => Set<Person>();
        public PeopleContext(DbContextOptions<PeopleContext> opts) : base(opts)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(p => new { p.Email, p.HashPassword });
        }
    }
}
