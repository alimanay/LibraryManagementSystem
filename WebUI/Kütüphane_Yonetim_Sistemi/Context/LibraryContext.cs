using Entites.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Kütüphane_Yonetim_Sistemi.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles{ get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            base.OnModelCreating(modelBuilder);
        }
    }
}