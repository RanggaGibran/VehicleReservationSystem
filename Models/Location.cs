namespace VehicleReservationSystem.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Headquarters, Branch, Mine
        public string Address { get; set; }
        public string Region { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Driver> Drivers { get; set; }

        // Update constructor:
        public Location()
        {
            Vehicles = new HashSet<Vehicle>();
            Users = new HashSet<ApplicationUser>();
            Drivers = new HashSet<Driver>();
        }
    }
}