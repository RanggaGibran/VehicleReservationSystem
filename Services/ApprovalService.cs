using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;

namespace VehicleReservationSystem.Services
{
    public interface IApprovalService
    {
        Task<int> CreateApproval(int reservationId, string approverId, int level);
        Task<bool> ApproveReservation(int approvalId, string approverId, string comments);
        Task<bool> RejectReservation(int approvalId, string approverId, string comments);
        Task<bool> UpdateOtherApprovals(int reservationId, int approvalId, string status, string comment);
    }    public class ApprovalService : IApprovalService
    {
        private readonly AppDbContext _context;

        public ApprovalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateOtherApprovals(int reservationId, int approvalId, string status, string comment)
        {
            var otherPendingApprovals = await _context.Approvals
                .Where(a => a.ReservationId == reservationId && a.Status == "Pending" && a.Id != approvalId)
                .ToListAsync();

            foreach (var pendingApproval in otherPendingApprovals)
            {
                pendingApproval.Status = status;
                pendingApproval.ActionDate = DateTime.Now;
                pendingApproval.Comments = comment;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveReservation(int approvalId, string approverId, string comments)
        {
            var approval = await _context.Approvals
                .Include(a => a.Reservation)
                .FirstOrDefaultAsync(a => a.Id == approvalId && a.ApproverId == approverId);

            if (approval == null)
                return false;

            approval.Status = "Approved";
            approval.ActionDate = DateTime.Now;
            approval.Comments = comments;

            // Check if this is the last approval needed
            var allApprovals = await _context.Approvals
                .Where(a => a.ReservationId == approval.ReservationId)
                .ToListAsync();

            var pendingHigherLevelApprovals = allApprovals
                .Where(a => a.Level > approval.Level && a.Status == "Pending")
                .ToList();            // If no more pending higher level approvals, update reservation status
            if (pendingHigherLevelApprovals.Count == 0)
            {
                var reservation = await _context.Reservations.FindAsync(approval.ReservationId);
                if (reservation != null)
                {
                    reservation.Status = "Approved";
                }
            }

            _context.Approvals.Update(approval);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectReservation(int approvalId, string approverId, string comments)
        {
            var approval = await _context.Approvals
                .Include(a => a.Reservation)
                .FirstOrDefaultAsync(a => a.Id == approvalId && a.ApproverId == approverId);

            if (approval == null)
                return false;

            approval.Status = "Rejected";
            approval.ActionDate = DateTime.Now;
            approval.Comments = comments;            // Reject the reservation
            var reservation = await _context.Reservations.FindAsync(approval.ReservationId);
            if (reservation != null)
            {
                reservation.Status = "Rejected";
                var vehicle = await _context.Vehicles.FindAsync(reservation.VehicleId);
                if (vehicle != null)
                {
                    vehicle.IsAvailable = true;
                }
            }

            // Update all other pending approvals for this reservation to "Cancelled"
            await UpdateOtherApprovals(
                approval.ReservationId, 
                approvalId, 
                "Cancelled", 
                "Cancelled due to rejection at level " + approval.Level);

            _context.Approvals.Update(approval);
            await _context.SaveChangesAsync();

            return true;        }

        public async Task<int> CreateApproval(int reservationId, string approverId, int level)
        {
            var approval = new Approval
            {
                ReservationId = reservationId,
                ApproverId = approverId,
                Level = level,
                Status = "Pending",
                Comments = string.Empty
            };

            _context.Approvals.Add(approval);
            await _context.SaveChangesAsync();

            return approval.Id;
        }
    }
}