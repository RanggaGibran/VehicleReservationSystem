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
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public int NumberOfPassengers { get; set; }
        public string Status { get; set; } = "Pending";
        public string Priority { get; set; } = "Normal"; // Normal, High, Urgent
        public string ProjectCode { get; set; } = string.Empty; // For mining projects
        public decimal? EstimatedDistance { get; set; } // In kilometers
        public string LoadDescription { get; set; } = string.Empty; // For cargo vehicles
        public decimal? LoadWeight { get; set; } // In tons
        public string SpecialRequirements { get; set; } = string.Empty;
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public int? ActualMileage { get; set; }
        public string CompletionNotes { get; set; } = string.Empty;
        public ICollection<Approval> Approvals { get; set; } = new HashSet<Approval>();
    }
}