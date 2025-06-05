namespace VehicleReservationSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string RequesterId { get; set; }
        public ApplicationUser Requester { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int? DriverId { get; set; }
        public Driver Driver { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Purpose { get; set; }
        public string Destination { get; set; }
        public int NumberOfPassengers { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected, Completed
        public ICollection<Approval> Approvals { get; set; }

        // Update constructor:
        public Reservation()
        {
            Approvals = new HashSet<Approval>();
        }
    }
}