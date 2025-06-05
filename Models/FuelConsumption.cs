namespace VehicleReservationSystem.Models
{
    public class FuelConsumption
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public DateTime Date { get; set; }
        public double Liters { get; set; }
        public decimal Cost { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public double Odometer { get; set; }
        public string FillingStation { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}