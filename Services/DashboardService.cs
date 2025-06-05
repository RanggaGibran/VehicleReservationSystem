using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;
using VehicleReservationSystem.ViewModels;

namespace VehicleReservationSystem.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardData();
        Task<object> GetVehicleUsageChartData();
        Task<object> GetReservationStatusChartData();
        Task<object> GetFuelConsumptionChartData();
    }

    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }        public async Task<DashboardViewModel> GetDashboardData()
        {
            try
            {
                var totalVehicles = await _context.Vehicles.CountAsync();
                var availableVehicles = await _context.Vehicles.CountAsync(v => v.IsAvailable);
                var pendingReservations = await _context.Reservations.CountAsync(r => r.Status == "Pending");
                var activeReservations = await _context.Reservations.CountAsync(r => r.Status == "Approved" && r.EndDate >= DateTime.Today);

                var recentReservations = await _context.Reservations
                    .Include(r => r.Vehicle)
                        .ThenInclude(v => v.VehicleType)
                    .Include(r => r.Requester)
                    .Include(r => r.Driver)
                    .OrderByDescending(r => r.RequestDate)
                    .Take(5)
                    .ToListAsync();

                var upcomingServices = await _context.ServiceSchedules
                    .Include(s => s.Vehicle)
                        .ThenInclude(v => v.VehicleType)
                    .Where(s => s.ScheduleDate >= DateTime.Today && s.ScheduleDate <= DateTime.Today.AddDays(30))
                    .OrderBy(s => s.ScheduleDate)
                    .Take(5)
                    .ToListAsync();

                return new DashboardViewModel
                {
                    TotalVehicles = totalVehicles,
                    AvailableVehicles = availableVehicles,
                    PendingReservations = pendingReservations,
                    ActiveReservations = activeReservations,
                    RecentReservations = recentReservations ?? new List<Reservation>(),
                    UpcomingServices = upcomingServices ?? new List<ServiceSchedule>()
                };
            }
            catch (Exception ex)
            {
                // Log the error (in a real application, use proper logging)
                Console.WriteLine($"Error in GetDashboardData: {ex.Message}");
                
                // Return a default dashboard with zero values
                return new DashboardViewModel
                {
                    TotalVehicles = 0,
                    AvailableVehicles = 0,
                    PendingReservations = 0,
                    ActiveReservations = 0,
                    RecentReservations = new List<Reservation>(),
                    UpcomingServices = new List<ServiceSchedule>()
                };
            }
        }        public async Task<object> GetVehicleUsageChartData()
        {
            try
            {
                var startDate = DateTime.Today.AddMonths(-1);
                var endDate = DateTime.Today;

                var vehicleUsage = await _context.Reservations
                    .Include(r => r.Vehicle)
                    .Where(r => r.Status == "Approved" || r.Status == "Completed")
                    .Where(r => r.StartDate >= startDate && r.StartDate <= endDate)
                    .GroupBy(r => r.Vehicle.RegistrationNumber)
                    .Select(g => new
                    {
                        Vehicle = g.Key,
                        UsageCount = g.Count()
                    })
                    .OrderByDescending(x => x.UsageCount)
                    .Take(10)
                    .ToListAsync();

                return new
                {
                    Labels = vehicleUsage.Select(v => v.Vehicle).ToArray(),
                    Data = vehicleUsage.Select(v => v.UsageCount).ToArray()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetVehicleUsageChartData: {ex.Message}");
                return new
                {
                    Labels = new string[0],
                    Data = new int[0]
                };
            }
        }        public async Task<object> GetReservationStatusChartData()
        {
            try
            {
                var statusCounts = await _context.Reservations
                    .GroupBy(r => r.Status)
                    .Select(g => new
                    {
                        Status = g.Key,
                        Count = g.Count()
                    })
                    .ToListAsync();

                return new
                {
                    Labels = statusCounts.Select(s => s.Status).ToArray(),
                    Data = statusCounts.Select(s => s.Count).ToArray()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetReservationStatusChartData: {ex.Message}");
                return new
                {
                    Labels = new string[0],
                    Data = new int[0]
                };
            }
        }        public async Task<object> GetFuelConsumptionChartData()
        {
            try
            {
                var startDate = DateTime.Today.AddMonths(-6);
                var endDate = DateTime.Today;

                // First fetch the raw data without string formatting
                var rawData = await _context.FuelConsumptions
                    .Where(f => f.Date >= startDate && f.Date <= endDate)
                    .GroupBy(f => new { Month = f.Date.Month, Year = f.Date.Year })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalLiters = g.Sum(f => f.Liters),
                        TotalCost = g.Sum(f => f.Cost)
                    })
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ToListAsync();

                // Then format the strings client-side
                var monthlyConsumption = rawData
                    .Select(m => new
                    {
                        MonthYear = $"{m.Year}-{m.Month}",
                        TotalLiters = m.TotalLiters,
                        TotalCost = m.TotalCost
                    })
                    .ToList();

                return new
                {
                    Labels = monthlyConsumption.Select(m => m.MonthYear).ToArray(),
                    LitersData = monthlyConsumption.Select(m => m.TotalLiters).ToArray(),
                    CostData = monthlyConsumption.Select(m => m.TotalCost).ToArray()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFuelConsumptionChartData: {ex.Message}");
                return new
                {
                    Labels = new string[0],
                    LitersData = new double[0],
                    CostData = new decimal[0]
                };
            }
        }
    }
}