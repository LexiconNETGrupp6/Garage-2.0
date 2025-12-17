using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Garage_2._0.Models.ViewModels
{
    public class ReceiptViewModel
    {
        public Vehicle Vehicle { get; set; }

        [DisplayName("Time at checkout")]
        public DateTime CheckOutTime { get; set; } = DateTime.Now;

        [Range(5, 200)]
        [DisplayName("Hourly price")]
        public double PricePerHour { get; private set; } = 30;

        [DisplayName("Whole parking duration")]
        public TimeSpan ParkedDuration => CheckOutTime.Subtract(Vehicle.ArrivalTime);

        [DisplayName("Total price for parking")]
        public double TotalPrice => ParkedDuration.TotalHours * PricePerHour;
    }
}
