namespace Garage_2._0.Models
{
    public class ParkingSpot
    {
        public int Id { get; set; }
        public int GarageId { get; set; }
        public ParkingGarage Garage { get; set; } = null!;

        public int Floor { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public bool IsParkingSpace { get; set; }
        public bool IsBooked { get; set; }
        public string? BookedBy { get; set; }
        public DateTime? BookedAt { get; set; }
    }
}