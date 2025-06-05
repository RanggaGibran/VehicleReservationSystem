using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;
using VehicleReservationSystem.Services;
using VehicleReservationSystem.ViewModels;

namespace VehicleReservationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly AppDbContext _context;

        public ReportController(IReportService reportService, AppDbContext context)
        {
            _reportService = reportService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {            var reportViewModel = new ReportViewModel
            {
                StartDate = DateTime.Today.AddMonths(-1),
                EndDate = DateTime.Today,
                Locations = await GetLocationsSelectList(),
                VehicleTypes = await GetVehicleTypesSelectList(),
                ReportData = new()
            };
            
            return View(reportViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Generate(ReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var reportData = await _reportService.GenerateReservationReport(
                    model.StartDate, model.EndDate, model.LocationId, model.VehicleTypeId);
                
                model.ReportData = reportData;
            }
            
            // Repopulate dropdown lists
            model.Locations = await GetLocationsSelectList();
            model.VehicleTypes = await GetVehicleTypesSelectList();
            
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> ExportToExcel(DateTime startDate, DateTime endDate, int? locationId, int? vehicleTypeId)
        {
            var excelFile = await _reportService.ExportReservationReportToExcel(
                startDate, endDate, locationId, vehicleTypeId);
            
            return File(
                excelFile.Content,
                excelFile.ContentType,
                excelFile.FileName);
        }

        private async Task<List<SelectListItem>> GetLocationsSelectList()
        {
            return await _context.Locations
                .OrderBy(l => l.Name)
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = $"{l.Name} ({l.Region})"
                })
                .ToListAsync();
        }

        private async Task<List<SelectListItem>> GetVehicleTypesSelectList()
        {
            return await _context.VehicleTypes
                .OrderBy(vt => vt.Name)
                .Select(vt => new SelectListItem
                {
                    Value = vt.Id.ToString(),
                    Text = $"{vt.Name} ({vt.Category})"
                })
                .ToListAsync();
        }
    }
}