using Garage_2._0.ConstantStrings;
using System.ComponentModel;

namespace Garage_2._0.Models.ViewModels
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        
        [DisplayName("Type of Vehicle")]
        public VehicleType VehicleType { get; set; }

        [DisplayName("Registration Number")]
        public string RegNumber { get; set; } = string.Empty;

        [DisplayName("Time of Arrival")]
        public DateTime ArrivalTime { get; set; }

        [DisplayName("Parking Duration")]
        public String ParkDuration { get; private set; } = string.Empty;

        public void UpdateParkDuration()
        {
            var duration = DateTime.Now - ArrivalTime;
            var days = (int)duration.TotalDays;
            var timePart = duration.ToString(@"hh\:mm\:ss");
            ParkDuration = (days > 0 ? $"{days}d " : "") + timePart;
        }
    }
}
