using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace VehicleReservationSystem.ViewModels
{
    public class ReservationViewModel
    {
        [Required]
        public string RequesterId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Vehicle")]
        public int VehicleId { get; set; }

        [Display(Name = "Driver")]
        public int? DriverId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Purpose { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Number of Passengers")]
        [Range(1, 50)]
        public int NumberOfPassengers { get; set; }

        [Required]
        [Display(Name = "Approvers")]
        public List<string> ApproverIds { get; set; } = new();

        public List<SelectListItem> Vehicles { get; set; } = new();
        public List<SelectListItem> Drivers { get; set; } = new();
        public List<SelectListItem> Users { get; set; } = new();
        public List<SelectListItem> Approvers { get; set; } = new();
    }
}