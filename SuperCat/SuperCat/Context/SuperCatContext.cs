using Microsoft.EntityFrameworkCore;
using SuperCat.Configurate;
using SuperCat.MyObjects;
using System.Configuration;

namespace SuperCat.Context
{
    internal class SuperCatContext : DbContext
    {
        public SuperCatContext():base()
        { 

        }

        public DbSet<UserInfo> UsersInfo { get; set; } = null!;
        public DbSet<Friend> Friends { get; set; } = null!;
        public DbSet<UserImage> UserImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friend>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserImage>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<UserImage>().Property(x => x.UserId).HasColumnOrder(1);

            modelBuilder.ApplyConfiguration(new ConfigUserInfo());
        }
    }
}