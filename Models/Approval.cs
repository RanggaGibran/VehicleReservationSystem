namespace VehicleReservationSystem.Models
{
    public class Approval
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public string ApproverId { get; set; }
        public ApplicationUser Approver { get; set; }
        public int Level { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public DateTime? ActionDate { get; set; }
        public string Comments { get; set; }
    }
}