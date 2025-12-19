using System.ComponentModel;

namespace Garage_2._0.Models.ViewModels
{
    public class VehicleViewModel
    {
        public int Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public string RegNumber { get; set; } = string.Empty;
        public DateTime ArrivalTime { get; set; }

        [DisplayName("How long the vehicle has been parked")]
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
