using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;
using VehicleReservationSystem.Services;
using VehicleReservationSystem.ViewModels;

namespace VehicleReservationSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IReservationService _reservationService;
        private readonly IApprovalService _approvalService;

        public ReservationController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IReservationService reservationService,
            IApprovalService approvalService)
        {
            _context = context;
            _userManager = userManager;
            _reservationService = reservationService;
            _approvalService = approvalService;
        }        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (isAdmin)
            {
                var allReservations = await _context.Reservations
                    .Include(r => r.Vehicle)
                    .Include(r => r.Driver)
                    .Include(r => r.Requester)
                    .ToListAsync();
                return View(allReservations);
            }
            else
            {
                var userReservations = await _context.Reservations
                    .Include(r => r.Vehicle)
                    .Include(r => r.Driver)
                    .Include(r => r.Requester)
                    .Where(r => r.RequesterId == currentUser.Id)
                    .ToListAsync();
                return View(userReservations);
            }
        }        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var now = DateTime.Now;
            var viewModel = new ReservationViewModel
            {
                // Initialize with valid default dates to avoid HTML5 validation issues
                StartDate = now.AddHours(1), // Default to 1 hour from now
                EndDate = now.AddHours(5),   // Default to 5 hours from now
                
                Vehicles = await _context.Vehicles
                    .Where(v => v.IsAvailable)
                    .Select(v => new SelectListItem
                    {
                        Value = v.Id.ToString(),
                        Text = $"{v.Brand} {v.Model} - {v.RegistrationNumber}"
                    }).ToListAsync(),

                Drivers = await _context.Drivers
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToListAsync(),

                Users = await _userManager.Users
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.FullName
                    }).ToListAsync(),

                Approvers = await _userManager.GetUsersInRoleAsync("Approver")
                    .ContinueWith(t => t.Result
                        .Select(u => new SelectListItem
                        {
                            Value = u.Id,
                            Text = $"{u.FullName} - {u.Position}"
                        }).ToList())
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _reservationService.CreateReservation(model);
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown lists if validation fails
            model.Vehicles = await _context.Vehicles
                .Where(v => v.IsAvailable)
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.Brand} {v.Model} - {v.RegistrationNumber}"
                }).ToListAsync();

            model.Drivers = await _context.Drivers
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToListAsync();

            model.Users = await _userManager.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName
                }).ToListAsync();

            model.Approvers = await _userManager.GetUsersInRoleAsync("Approver")
                .ContinueWith(t => t.Result
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = $"{u.FullName} - {u.Position}"
                    }).ToList());

            return View(model);
        }        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Vehicle)
                .Include(r => r.Driver)
                .Include(r => r.Requester)
                .Include(r => r.Approvals)
                .ThenInclude(a => a.Approver)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Check authorization - users can only view their own reservations unless they're admin
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            
            if (!isAdmin && reservation.RequesterId != currentUser.Id)
            {
                return Forbid();
            }            return View(reservation);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Vehicle)
                .Include(r => r.Driver)
                .Include(r => r.Requester)
                .Include(r => r.Approvals)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Only allow editing of pending reservations
            if (reservation.Status != "Pending")
            {
                TempData["Error"] = "Hanya reservasi dengan status 'Menunggu' yang dapat diedit.";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Check authorization - users can only edit their own reservations unless they're admin
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            
            if (!isAdmin && reservation.RequesterId != currentUser.Id)
            {
                return Forbid();
            }

            // Prepare view model
            var now = DateTime.Now;
            var viewModel = new ReservationViewModel
            {
                RequesterId = reservation.RequesterId,
                VehicleId = reservation.VehicleId,
                DriverId = reservation.DriverId,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                Purpose = reservation.Purpose,
                Destination = reservation.Destination,
                NumberOfPassengers = reservation.NumberOfPassengers,
                
                Vehicles = await _context.Vehicles
                    .Where(v => v.IsAvailable || v.Id == reservation.VehicleId)
                    .Select(v => new SelectListItem
                    {
                        Value = v.Id.ToString(),
                        Text = $"{v.Brand} {v.Model} - {v.RegistrationNumber}",
                        Selected = v.Id == reservation.VehicleId
                    }).ToListAsync(),

                Drivers = await _context.Drivers
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name,
                        Selected = d.Id == reservation.DriverId
                    }).ToListAsync(),

                Users = await _userManager.Users
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = u.FullName,
                        Selected = u.Id == reservation.RequesterId
                    }).ToListAsync(),

                Approvers = await _userManager.GetUsersInRoleAsync("Approver")
                    .ContinueWith(t => t.Result
                        .Select(u => new SelectListItem
                        {
                            Value = u.Id,
                            Text = $"{u.FullName} - {u.Position}",
                            Selected = reservation.Approvals.Any(a => a.ApproverId == u.Id)
                        }).ToList()),
                
                ApproverIds = reservation.Approvals.OrderBy(a => a.Level).Select(a => a.ApproverId).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationViewModel model)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Approvals)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Only allow editing of pending reservations
            if (reservation.Status != "Pending")
            {
                TempData["Error"] = "Hanya reservasi dengan status 'Menunggu' yang dapat diedit.";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Check authorization - users can only edit their own reservations unless they're admin
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }
            
            var isAdmin = await _userManager.IsInRoleAsync(currentUser, "Admin");
            
            if (!isAdmin && reservation.RequesterId != currentUser.Id)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update reservation details
                    reservation.RequesterId = model.RequesterId;
                    reservation.VehicleId = model.VehicleId;
                    reservation.DriverId = model.DriverId;
                    reservation.StartDate = model.StartDate;
                    reservation.EndDate = model.EndDate;
                    reservation.Purpose = model.Purpose;
                    reservation.Destination = model.Destination;
                    reservation.NumberOfPassengers = model.NumberOfPassengers;

                    // Update vehicle availability if vehicle changed
                    var oldVehicle = await _context.Vehicles.FindAsync(reservation.VehicleId);
                    if (oldVehicle != null && oldVehicle.Id != model.VehicleId)
                    {
                        oldVehicle.IsAvailable = true;
                    }

                    var newVehicle = await _context.Vehicles.FindAsync(model.VehicleId);
                    if (newVehicle != null && newVehicle.IsAvailable)
                    {
                        newVehicle.IsAvailable = false;
                    }

                    // Remove existing approvals
                    _context.Approvals.RemoveRange(reservation.Approvals);

                    // Add new approvals
                    for (int i = 0; i < model.ApproverIds.Count; i++)
                    {
                        var approval = new Approval
                        {
                            ReservationId = reservation.Id,
                            ApproverId = model.ApproverIds[i],
                            Level = i + 1,
                            Status = "Pending"
                        };
                        _context.Approvals.Add(approval);
                    }

                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Reservasi berhasil diperbarui.";
                    return RedirectToAction(nameof(Details), new { id });
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Terjadi kesalahan saat memperbarui reservasi: " + ex.Message;
                }
            }

            // Repopulate dropdown lists if validation fails
            model.Vehicles = await _context.Vehicles
                .Where(v => v.IsAvailable || v.Id == reservation.VehicleId)
                .Select(v => new SelectListItem
                {
                    Value = v.Id.ToString(),
                    Text = $"{v.Brand} {v.Model} - {v.RegistrationNumber}"
                }).ToListAsync();

            model.Drivers = await _context.Drivers
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToListAsync();

            model.Users = await _userManager.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = u.FullName
                }).ToListAsync();

            model.Approvers = await _userManager.GetUsersInRoleAsync("Approver")
                .ContinueWith(t => t.Result
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = $"{u.FullName} - {u.Position}"
                    }).ToList());

            return View(model);
        }
    }
}