using Microsoft.EntityFrameworkCore;
using SuperCat.Configurate;
using SuperCat.MyObjects;
using System.Configuration;

namespace SuperCat.Context
{
    internal class SuperCatContext : DbContext
    {
        public DbSet<UserInfo> UsersInfo { get; set; } = null!;
        public DbSet<MyImage> MyImages { get; set; } = null!;
        public DbSet<Friend> Friends { get; set; } = null!;
        public DbSet<GroupInfo> GroupsInfo { get; set; } = null!;
        public DbSet<GroupMember> GroupMembers { get; set; } = null!;
        public DbSet<ChatInfo> ChatsInfo { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<MyImage>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Friend>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<GroupInfo>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<GroupMember>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ChatInfo>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}