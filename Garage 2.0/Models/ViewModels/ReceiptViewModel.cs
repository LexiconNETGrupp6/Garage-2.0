using Garage_2._0.ConstantValues;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [DisplayName("Whole parking duration")]
        public TimeSpan ParkedDuration => CheckOutTime.Subtract(ArrivalTime);

        [DisplayName("Total price for parking")]
        public double TotalPrice => ParkedDuration.TotalHours * PriceConsts.HOURLY_PARKING_PRICE;
    }
}
