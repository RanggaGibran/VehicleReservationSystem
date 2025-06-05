namespace VehicleReservationSystem.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // People or Goods
        public int Capacity { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }

        // Update constructor:
        public VehicleType()
        {
            Vehicles = new HashSet<Vehicle>();
        }
    }
}