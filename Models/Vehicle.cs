namespace VehicleReservationSystem.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; } = null!;
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
        public bool IsCompanyOwned { get; set; } = true;
        public string RentalCompany { get; set; } = string.Empty;
        public DateTime? RentalStartDate { get; set; }
        public DateTime? RentalEndDate { get; set; }
        public decimal? RentalCostPerDay { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string Status { get; set; } = "Active";
        public DateTime? LastServiceDate { get; set; }
        public DateTime? NextServiceDate { get; set; }
        public int Mileage { get; set; }
        public string FuelType { get; set; } = "Solar"; // Solar, Bensin, etc
        public decimal FuelCapacity { get; set; }
        public string Notes { get; set; } = string.Empty;
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        public ICollection<FuelConsumption> FuelConsumptions { get; set; } = new HashSet<FuelConsumption>();
        public ICollection<ServiceSchedule> ServiceSchedules { get; set; } = new HashSet<ServiceSchedule>();
    }
}