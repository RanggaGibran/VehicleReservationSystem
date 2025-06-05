using Microsoft.AspNetCore.Identity;

namespace VehicleReservationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            FullName = string.Empty;
            Position = string.Empty;
            Department = string.Empty;
        }

        public string FullName { get; set; }
        public int? LocationId { get; set; }
        public Location? Location { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string? SupervisorId { get; set; }
        public ApplicationUser? Supervisor { get; set; }
    }
}