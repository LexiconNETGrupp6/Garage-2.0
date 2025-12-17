using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string RegNumber { get; set; } = string.Empty;

        [Required]
        public VehicleType VehicleType { get; set; }

        [StringLength(30)]
        public string Color { get; set; } = string.Empty;

        [StringLength(30)]
        public string Brand { get; set; } = string.Empty;

        [StringLength(30)]
        public string Model { get; set; } = string.Empty;

        [Range(0, 50)]
        public int NumberOfWheels { get; set; }

        public DateTime ArrivalTime { get; set; }
    }
}
