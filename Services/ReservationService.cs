using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;
using VehicleReservationSystem.ViewModels;

namespace VehicleReservationSystem.Services
{
    public interface IReservationService
    {
        Task<int> CreateReservation(ReservationViewModel model);
        Task<bool> UpdateReservationStatus(int reservationId, string status);
    }    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateReservation(ReservationViewModel model)
        {
            var requester = await _context.Users.FindAsync(model.RequesterId);
            if (requester == null)
                throw new InvalidOperationException("Requester not found");

            var vehicle = await _context.Vehicles.FindAsync(model.VehicleId);
            if (vehicle == null || !vehicle.IsAvailable)
                throw new InvalidOperationException("Vehicle not available");

            if (model.DriverId.HasValue)
            {
                var driver = await _context.Drivers.FindAsync(model.DriverId.Value);
                if (driver == null)
                    throw new InvalidOperationException("Driver not found");
            }

            foreach (var approverId in model.ApproverIds)
            {
                var approver = await _context.Users.FindAsync(approverId);
                if (approver == null)
                    throw new InvalidOperationException("Approver not found");
            }

            var reservation = new Reservation
            {
                RequesterId = model.RequesterId,
                VehicleId = model.VehicleId,
                DriverId = model.DriverId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Purpose = model.Purpose,
                Destination = model.Destination,
                NumberOfPassengers = model.NumberOfPassengers
            };            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

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

            vehicle.IsAvailable = false;
            await _context.SaveChangesAsync();

            return reservation.Id;
        }

        public async Task<bool> UpdateReservationStatus(int reservationId, string status)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null) return false;

            reservation.Status = status;

            if (status is "Completed" or "Rejected")
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