namespace VehicleReservationSystem.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "HeadOffice", "BranchOffice", "MiningLocation"
        public string Address { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string MineCode { get; set; } = string.Empty; // For mining locations
        public bool IsActive { get; set; } = true;
        public string ContactPerson { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<Vehicle> Vehicles { get; set; } = new HashSet<Vehicle>();
        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
        public ICollection<Driver> Drivers { get; set; } = new HashSet<Driver>();
    }
}