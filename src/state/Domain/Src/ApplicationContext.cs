using System.Reflection;
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
            modelBuilder.ApplyConfiguration(new AttemptHistoryDbConfiguration());
            modelBuilder.ApplyConfiguration(new AttemptDbConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<WordDto> Words { get; set; }

        public DbSet<AttemptHistoryDto> AttemptHistories { get; set; }

        public DbSet<AttemptDto> Attempts { get; set; }
    }
}
