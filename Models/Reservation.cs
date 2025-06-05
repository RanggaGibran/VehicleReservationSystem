namespace VehicleReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string RequesterId { get; set; } = string.Empty;
        public ApplicationUser Requester { get; set; } = null!;
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int NumberOfPassengers { get; set; }
        public string Status { get; set; } = "Pending";
        public ICollection<Approval> Approvals { get; set; } = new HashSet<Approval>();
    }
}