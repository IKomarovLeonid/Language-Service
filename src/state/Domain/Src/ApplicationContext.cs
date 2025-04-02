using System.Reflection;
using Domain.Configuration;
using Domain.Src.Configuration;
using Microsoft.EntityFrameworkCore;
using Objects.Dto;
using Objects.Src.Dto;
using Objects.Src.Models;

namespace Domain
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=database.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=database.db");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WordsDbConfiguration());
            modelBuilder.ApplyConfiguration(new GameAttemptDbConfiguration());
            modelBuilder.ApplyConfiguration(new UserStatisticsDbConfiguration());
            modelBuilder.ApplyConfiguration(new UsersDbConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<WordDto> Words { get; set; }

        public DbSet<GameAttemptDto> GameAttempts { get; set; }

        public DbSet<UserDto> Users { get; set; }

        public DbSet<UserStatisticsDto> UserStatistics { get; set; }
    }
}
