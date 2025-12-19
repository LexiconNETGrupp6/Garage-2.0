using Garage_2._0.ConstantStrings;
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

        public string VehicleTypeSortOrder { get; set; } = string.Empty;
        public string RegNumberSortOrder { get; set; } = string.Empty;
        public string ArrivalTimeSortOrder { get; set; } = string.Empty;
        public string DurationSortOrder { get; set; } = string.Empty;

        // <Name, isAscending>
        public static Dictionary<string, bool> VehicleSortCategories { get; set; } = new() {
            { VehicleViewModelSortingCategories.VehicleType, true },
            { VehicleViewModelSortingCategories.RegNumber, true },
            { VehicleViewModelSortingCategories.ArrivalTime, true },
            { VehicleViewModelSortingCategories.Duration, true }
        };

        public void UpdateParkDuration()
        {
            var duration = DateTime.Now - ArrivalTime;
            var days = (int)duration.TotalDays;
            var timePart = duration.ToString(@"hh\:mm\:ss");
            ParkDuration = (days > 0 ? $"{days}d " : "") + timePart;
        }    
        
        // Sorts either ascending or descdening using the sent in condition
        public static IEnumerable<VehicleViewModel> Sort(IEnumerable<VehicleViewModel> vehicles, Func<VehicleViewModel, string> condition, bool isAscending = true)
        {
            if (isAscending)
                return vehicles.OrderBy(condition);

            return vehicles.OrderByDescending(condition);
        }
    }
}
