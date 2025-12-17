using System.ComponentModel;

namespace Garage_2._0.Models.ViewModels
{
    public class VehicleViewModel
    {
        public VehicleType Type { get; set; }
        public string RegNumber { get; set; } = string.Empty;
        public DateTime ArrivalTime { get; set; }

        [DisplayName("How long the vehicle has been parked")]
        public TimeSpan ParkDuration { get; private set; }

        public void UpdateParkDuration()        
            => ParkDuration = DateTime.Now.Subtract(ArrivalTime);        
    }
}
