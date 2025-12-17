namespace Garage_2._0.Models.ViewModels
{
    public class VehicleViewModel
    {
        public VehicleType Type { get; set; }
        public string RegNumber { get; set; } = string.Empty;
        public DateTime ArrivalTime { get; set; }
    }
}
