namespace VehicleReservationSystem.Models
{
    public class ServiceSchedule
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string ServiceType { get; set; } // Regular, Maintenance, Repair
        public string Description { get; set; }
        public string ServiceProvider { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Status { get; set; } // Scheduled, Completed, Cancelled
        public DateTime? CompletionDate { get; set; }
        public decimal? ActualCost { get; set; }
        public string Notes { get; set; }
    }
}