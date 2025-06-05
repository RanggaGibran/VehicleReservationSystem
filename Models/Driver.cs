namespace VehicleReservationSystem.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpiry { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
        public bool IsAvailable { get; set; } = true;
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
    }
}