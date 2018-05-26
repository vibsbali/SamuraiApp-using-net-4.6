using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
   public class SamuraiContext : DbContext
   {
      public static readonly LoggerFactory MyConsoleLoggerFactory = new LoggerFactory(
         new []
         {
            new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name
            && level == LogLevel.Information, true) 
         });

      public DbSet<Samurai> Samurais { get; set; }
      public DbSet<Battle> Battles { get; set; }
      public DbSet<Quote> Quotes { get; set; }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
         optionsBuilder
            .UseLoggerFactory(MyConsoleLoggerFactory)
            .EnableSensitiveDataLogging(true) //<- This line will show the sensitive information in log
            .UseSqlServer("Server = (localdb)\\mssqllocaldb;Database = SamuraiApp;Trusted_Connection = True;");
         
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         //composite key
         modelBuilder.Entity<SamuraiBattle>()
            .HasKey(s => new {s.SamuraiId, s.BattleId});
         modelBuilder.Entity<Battle>().Property(b => b.StartDate).HasColumnType("Date");
         modelBuilder.Entity<Battle>().Property(b => b.EndDate).HasColumnType("Date");

         //Add Shadow properties
         modelBuilder.Entity<Samurai>().Property<DateTime>("Created");
         modelBuilder.Entity<Samurai>().Property<DateTime>("LastModified");

         //Iterative Way for adding Shadow Properties to all the entities
         foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
         {
            modelBuilder.Entity(mutableEntityType.Name).Property<DateTime>("Created");
            modelBuilder.Entity(mutableEntityType.Name).Property<DateTime>("LastModified");
         }
      }

      public override int SaveChanges()
      {
         ChangeTracker.DetectChanges();
         var timeStamp = DateTime.Now;

         foreach (var entry in ChangeTracker.Entries())
         {
            entry.Property("LastModified").CurrentValue = timeStamp;

            if (entry.State == EntityState.Added)
            {
               entry.Property("Created").CurrentValue = timeStamp;
            }
         }

         return base.SaveChanges();
      }
   }
}
