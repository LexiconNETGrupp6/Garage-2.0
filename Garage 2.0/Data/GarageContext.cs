using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;

namespace Garage_2._0.Data
{
    public class GarageContext : DbContext
    {
        public GarageContext(DbContextOptions<GarageContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicle { get; set; } = default!;
        public DbSet<ParkingGarage> Garages { get; set; } = default!;
        public DbSet<ParkingSpot> ParkingSpots { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { Id = 1, RegNumber = "ABC123", VehicleType = VehicleType.Car, Color = "Red", Brand = "Volvo", Model = "V70", NumberOfWheels = 4 },
                new Vehicle { Id = 2, RegNumber = "LGH436", VehicleType = VehicleType.Boat, Color = "Yellow", Brand = "East Marine", Model = "Viking Line", NumberOfWheels = 0 },
                new Vehicle { Id = 3, RegNumber = "AHC745", VehicleType = VehicleType.Bus, Color = "Red", Brand = "Volvo", Model = "V7900", NumberOfWheels = 8 },
                new Vehicle { Id = 4, RegNumber = "KAK156", VehicleType = VehicleType.Car, Color = "White", Brand = "Teesla", Model = "X", NumberOfWheels = 4 },
                new Vehicle { Id = 5, RegNumber = "IKA71U", VehicleType = VehicleType.Truck, Color = "Blue", Brand = "Scania", Model = "G-series", NumberOfWheels = 6 },
                new Vehicle { Id = 6, RegNumber = "ÅJAUIV", VehicleType = VehicleType.Motorcycle, Color = "Green", Brand = "Mercedez", Model = "L420", NumberOfWheels = 2 }
            );

            modelBuilder.Entity<ParkingGarage>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Name).HasMaxLength(100);
                entity.Property(g => g.LayoutJson).IsRequired();
            });

            modelBuilder.Entity<ParkingSpot>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => new { s.GarageId, s.Floor, s.Row, s.Column }).IsUnique();
                entity.HasOne(s => s.Garage).WithMany(g => g.Spots).HasForeignKey(s => s.GarageId);
            });

        }
    }
}