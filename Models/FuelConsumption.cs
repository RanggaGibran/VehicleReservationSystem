namespace VehicleReservationSystem.Models
{
    public class FuelConsumption
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime Date { get; set; }
        public double Liters { get; set; }
        public decimal Cost { get; set; }
        public string FuelType { get; set; }
        public double Odometer { get; set; }
        public string FillingStation { get; set; }
        public string Notes { get; set; }
    }
}