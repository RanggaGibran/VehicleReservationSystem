using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;
using VehicleReservationSystem.ViewModels;

namespace VehicleReservationSystem.Services
{
    public interface IReservationService
    {
        Task<int> CreateReservation(ReservationViewModel model);
        Task<bool> UpdateReservationStatus(int reservationId, string status);
    }

    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;
        private readonly Lazy<IApprovalService> _approvalService;

        public ReservationService(AppDbContext context, Lazy<IApprovalService> approvalService)
        {
            _context = context;
            _approvalService = approvalService;
        }

        public async Task<int> CreateReservation(ReservationViewModel model)
        {
            // Verify the requester exists
            var requester = await _context.Users.FindAsync(model.RequesterId);
            if (requester == null)
            {
                throw new Exception("Selected requester does not exist");
            }

            // Verify the vehicle exists and is available
            var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            if (vehicle == null)
            {
                throw new Exception("Selected vehicle does not exist");
            }
            
            if (!vehicle.IsAvailable)
            {
                throw new Exception("Selected vehicle is not available");
            }

            // Verify the driver exists if one is selected
            if (model.DriverId.HasValue)
            {
                var driver = await _context.Drivers.FindAsync(model.DriverId.Value);
                if (driver == null)
                {
                    throw new Exception("Selected driver does not exist");
                }
            }

            // Verify all approvers exist
            foreach (var approverId in model.ApproverIds)
            {
                var approver = await _context.Users.FindAsync(approverId);
                if (approver == null)
                {
                    throw new Exception($"One of the selected approvers does not exist");
                }
            }

            var reservation = new Reservation
            {
                RequesterId = model.RequesterId,
                VehicleId = model.VehicleId,
                DriverId = model.DriverId,
                RequestDate = DateTime.Now,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Purpose = model.Purpose,
                Destination = model.Destination,
                NumberOfPassengers = model.NumberOfPassengers,
                Status = "Pending",
                Approvals = new List<Approval>()
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Create approval workflow with selected approvers
            for (int i = 0; i < model.ApproverIds.Count; i++)
            {
                await _approvalService.Value.CreateApproval(
                    reservation.Id, 
                    model.ApproverIds[i], 
                    i + 1);
            }

            // Update vehicle availability
            vehicle.IsAvailable = false;
            await _context.SaveChangesAsync();

            return reservation.Id;
        }

        public async Task<bool> UpdateReservationStatus(int reservationId, string status)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            
            if (reservation == null)
                return false;
                
            reservation.Status = status;
            
            if (status == "Completed" || status == "Rejected")
            {
                var vehicle = await _context.Vehicles.FindAsync(reservation.VehicleId);
                if (vehicle != null)
                {
                    vehicle.IsAvailable = true;
                    _context.Vehicles.Update(vehicle);
                }
            }
            
            _context.Reservations.Update(reservation);
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}