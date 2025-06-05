namespace VehicleReservationSystem.Models
{
    public class ServiceSchedule
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public DateTime ScheduleDate { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ServiceProvider { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public string Status { get; set; } = "Scheduled";
        public DateTime? CompletionDate { get; set; }
        public decimal? ActualCost { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}