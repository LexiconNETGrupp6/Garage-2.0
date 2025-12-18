namespace Garage_2._0.Models.ViewModels
{
    public class DeleteVehicleViewModel
    {
        public int Id { get; set; }
        public string RegNumber { get; set; } = string.Empty;
        public VehicleType VehicleType { get; set; }
        public bool WantReceipt { get; set; }
    }
}
