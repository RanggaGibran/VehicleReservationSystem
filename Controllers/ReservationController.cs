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
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
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
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ReservationViewModel
            {
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
        }
    }
}