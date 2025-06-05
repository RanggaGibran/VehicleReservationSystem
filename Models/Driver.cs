namespace VehicleReservationSystem.Models
{
    public class Driver
    {
        public Driver()
        {
            Reservations = new HashSet<Reservation>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseExpiry { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}