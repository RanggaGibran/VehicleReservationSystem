namespace VehicleReservationSystem.Models
{
    public class Vehicle
    {
        public Vehicle()
        {
            Reservations = new HashSet<Reservation>();
            FuelConsumptions = new HashSet<FuelConsumption>();
            ServiceSchedules = new HashSet<ServiceSchedule>();
        }
        
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int VehicleTypeId { get; set; }
        public VehicleType VehicleType { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public bool IsCompanyOwned { get; set; }
        public string RentalCompany { get; set; }
        public DateTime? RentalStartDate { get; set; }
        public DateTime? RentalEndDate { get; set; }
        public bool IsAvailable { get; set; }
        public string Status { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<FuelConsumption> FuelConsumptions { get; set; }
        public ICollection<ServiceSchedule> ServiceSchedules { get; set; }
    }
}