using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Garage_2._0.Models.ViewModels
{
    public class ReceiptViewModel
    {
        public string VehicleRegNumber { get; set; } = string.Empty;
        public VehicleType VehicleType { get; set; }

        [DisplayName("Time of Arrival")]
        public DateTime ArrivalTime { get; set; }

        [DisplayName("Time of checkout")]
        public DateTime CheckOutTime { get; set; } = DateTime.Now;

        [Range(5, 200)]
        [DisplayName("Hourly price")]
        public double PricePerHour { get; private set; } = 30;

        [DisplayName("Whole parking duration")]
        public TimeSpan ParkedDuration => CheckOutTime.Subtract(ArrivalTime);

        [DisplayName("Total price for parking")]
        public double TotalPrice => ParkedDuration.TotalHours * PricePerHour;
    }
}
