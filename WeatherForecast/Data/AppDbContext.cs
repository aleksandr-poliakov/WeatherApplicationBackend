using Microsoft.EntityFrameworkCore;
using WeatherForecast.Models;

namespace WeatherForecast.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
 {
     public DbSet<Subscription> Subscriptions => Set<Subscription>();
     public DbSet<Player> Players => Set<Player>();
     public DbSet<MatchDay> MatchDays => Set<MatchDay>();
     public DbSet<Match> Matches => Set<Match>();
     public DbSet<MatchParticipant> MatchParticipants => Set<MatchParticipant>();

     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         modelBuilder.Entity<MatchParticipant>()
             .HasKey(mp => new { mp.MatchId, mp.PlayerId });

         // tell EF to store MatchType as string not int
         modelBuilder.Entity<Match>()
             .Property(m => m.Type)
             .HasConversion<string>();
     }
 }