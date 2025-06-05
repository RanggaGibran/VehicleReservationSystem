using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace VehicleReservationSystem.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<FuelConsumption> FuelConsumptions { get; set; }
        public DbSet<ServiceSchedule> ServiceSchedules { get; set; }
    }
}