namespace VehicleReservationSystem.Models
{
    public class Approval
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
        public string ApproverId { get; set; } = string.Empty;
        public ApplicationUser Approver { get; set; } = null!;
        public int Level { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime? ActionDate { get; set; }
        public string Comments { get; set; } = string.Empty;
    }
}