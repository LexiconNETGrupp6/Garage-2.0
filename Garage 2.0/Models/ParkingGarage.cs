using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models
{
    public class ParkingGarage
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public int Floors { get; set; }
        public string LayoutJson { get; set; } = string.Empty; // { "0": [[true,false],[...]] }

        public ICollection<ParkingSpot> Spots { get; set; } = new List<ParkingSpot>();
    }
}
