using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleReservationSystem.ViewModels
{
    public class ReportViewModel
    {
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Location")]
        public int? LocationId { get; set; }

        [Display(Name = "Vehicle Type")]
        public int? VehicleTypeId { get; set; }

        public List<SelectListItem> Locations { get; set; } = new();
        public List<SelectListItem> VehicleTypes { get; set; } = new();
        public List<ReservationReportItemViewModel> ReportData { get; set; } = new();
    }

    public class ReservationReportItemViewModel
    {
        public int ReservationId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Duration { get; set; }
        public string VehicleRegistration { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string VehicleCategory { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string RequesterName { get; set; } = string.Empty;
        public string RequesterDepartment { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }    public class ExcelFileViewModel
    {
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}