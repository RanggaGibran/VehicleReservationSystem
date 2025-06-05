using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleReservationSystem.Services;
using VehicleReservationSystem.ViewModels;

namespace VehicleReservationSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardViewModel = await _dashboardService.GetDashboardData();
            return View(dashboardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetVehicleUsageData()
        {
            var data = await _dashboardService.GetVehicleUsageChartData();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservationStatusData()
        {
            var data = await _dashboardService.GetReservationStatusChartData();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetFuelConsumptionData()
        {
            var data = await _dashboardService.GetFuelConsumptionChartData();
            return Json(data);
        }
    }
}