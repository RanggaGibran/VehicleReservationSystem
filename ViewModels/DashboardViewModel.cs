using VehicleReservationSystem.Models;

namespace VehicleReservationSystem.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalVehicles { get; set; }
        public int AvailableVehicles { get; set; }
        public int PendingReservations { get; set; }
        public int ActiveReservations { get; set; }
        public List<Reservation> RecentReservations { get; set; } = new();
        public List<ServiceSchedule> UpcomingServices { get; set; } = new();
    }
}