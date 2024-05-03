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
            Database.EnsureCreated();
        }
        public DbSet<UserInfo> UsersInfo { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigUserInfo());
        }
    }
}