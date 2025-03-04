using Microsoft.EntityFrameworkCore;
using FileManagerLibrary.Models;

namespace FileManagerAPI.Models
{
    public class StorageContext : DbContext
    {
        public DbSet<FileManagerLibrary.Models.Directory> Directories => Set<FileManagerLibrary.Models.Directory>();
        public DbSet<FileManagerLibrary.Models.File> Files => Set<FileManagerLibrary.Models.File>();
        public StorageContext(DbContextOptions<StorageContext> opts) : base(opts)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileManagerLibrary.Models.Directory>().HasKey(d => new { d.Path, d.Name });
            modelBuilder.Entity<FileManagerLibrary.Models.File>().HasKey(f => new { f.Path, f.Name });
        }
    }
}
