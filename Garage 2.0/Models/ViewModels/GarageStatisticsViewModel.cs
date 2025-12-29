namespace Garage_2._0.Models.ViewModels
{
    public class GarageStatisticsViewModel
    {
        public int TotalVehicles { get; set; }

        public Dictionary<VehicleType, int> VehiclesByType { get; set; }
            = new Dictionary<VehicleType, int>();

        public int TotalWheels { get; set; }

        public double EstimatedRevenue { get; set; }
    }
}
