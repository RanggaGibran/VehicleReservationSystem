using Microsoft.AspNetCore.Identity;

namespace VehicleReservationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public int? LocationId { get; set; }
        public Location? Location { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string? SupervisorId { get; set; }
        public ApplicationUser? Supervisor { get; set; }
    }
}