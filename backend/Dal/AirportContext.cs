using Common.Models;
using Microsoft.EntityFrameworkCore;

using Models;
using System.Reflection;

namespace Dal
{
    public class AirportContext : DbContext
    {
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<StationsRelation> StationsRelation { get; set; }
        public DbSet<FlightStationDataToLog> FlightStationLogger { get; set; }
        public DbSet<StationState> AirportStationsState { get; set; }
        public AirportContext(DbContextOptions<AirportContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite("Data Source=AirportDB.db", option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<FlightStationDataToLog>().Property(fs => fs.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Station>().HasKey(s => s.Id);
            modelBuilder.Entity<Flight>().HasKey(f => f.Id);
            modelBuilder.Entity<StationsRelation>().HasKey(sr => new { sr.FromStationId, sr.ToStationId, sr.Direction });
            modelBuilder.Entity<StationState>().HasKey(s => s.StationId);

            modelBuilder.Entity<Flight>().Ignore(f => f.Name);
            modelBuilder
                .Entity<StationsRelation>()
                .HasOne(sr => sr.FromStation)
                .WithMany(s => s.StationsRelations).IsRequired();
                 
            modelBuilder.Entity<StationState>().HasOne(s => s.Station).WithOne(s => s.StationState).HasForeignKey<StationState>(s => s.StationId).IsRequired();
            modelBuilder.Entity<FlightStationDataToLog>().HasOne(f => f.Flight).WithMany();
            modelBuilder.Entity<StationState>().HasOne(s => s.Flight).WithOne(s => s.StationState).HasForeignKey<StationState>(s => s.FlightId);


            base.OnModelCreating(modelBuilder);
        }

    }
}
