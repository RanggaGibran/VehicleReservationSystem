namespace VehicleReservationSystem.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // "Passenger", "Cargo", "Heavy Equipment"
        public int Capacity { get; set; }
        public decimal LoadCapacity { get; set; } // For cargo vehicles (in tons)
        public bool RequiresSpecialLicense { get; set; } = false;
        public string FuelType { get; set; } = "Solar";
        public decimal AverageFuelConsumption { get; set; } // Liters per 100km
        public ICollection<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
    }
}