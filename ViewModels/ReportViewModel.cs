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

        public List<SelectListItem> Locations { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> VehicleTypes { get; set; } = new List<SelectListItem>();

        public List<ReservationReportItemViewModel> ReportData { get; set; } = new List<ReservationReportItemViewModel>();
    }

    public class ReservationReportItemViewModel
    {
        public int ReservationId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Duration { get; set; }
        public string VehicleRegistration { get; set; }
        public string VehicleType { get; set; }
        public string VehicleCategory { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public string RequesterName { get; set; }
        public string RequesterDepartment { get; set; }
        public string DriverName { get; set; }
        public string Purpose { get; set; }
        public string Destination { get; set; }
        public string Status { get; set; }
    }

    public class ExcelFileViewModel
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}