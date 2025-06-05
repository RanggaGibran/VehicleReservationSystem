using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;
using VehicleReservationSystem.Services;
using VehicleReservationSystem.ViewModels;

namespace VehicleReservationSystem.Controllers
{
    [Authorize(Roles = "Approver")]
    public class ApprovalController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApprovalService _approvalService;

        public ApprovalController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            IApprovalService approvalService)
        {
            _context = context;
            _userManager = userManager;
            _approvalService = approvalService;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            
            var pendingApprovals = await _context.Approvals
                .Include(a => a.Reservation)
                    .ThenInclude(r => r.Vehicle)
                .Include(a => a.Reservation)
                    .ThenInclude(r => r.Driver)
                .Include(a => a.Reservation)
                    .ThenInclude(r => r.Requester)
                .Where(a => a.ApproverId == currentUser.Id && a.Status == "Pending")
                .ToListAsync();
            
            return View(pendingApprovals);
        }

        public async Task<IActionResult> Details(int id)
        {
            var approval = await _context.Approvals
                .Include(a => a.Reservation)
                    .ThenInclude(r => r.Vehicle)
                .Include(a => a.Reservation)
                    .ThenInclude(r => r.Driver)
                .Include(a => a.Reservation)
                    .ThenInclude(r => r.Requester)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (approval == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (approval.ApproverId != currentUser.Id)
            {
                return Forbid();
            }

            return View(approval);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string comments)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            await _approvalService.ApproveReservation(id, currentUser.Id, comments);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string comments)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            await _approvalService.RejectReservation(id, currentUser.Id, comments);
            return RedirectToAction(nameof(Index));
        }
    }
}