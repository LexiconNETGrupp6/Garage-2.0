using Garage_2._0.ConstantStrings;
using Garage_2._0.Models;
using Garage_2._0.Models.Repositories;
using Garage_2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers
{
    public class GarageController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository; 
        private const int TotalParkingSpots = 10;
        public GarageController(IVehicleRepository repository)
        {
            _vehicleRepository = repository;
        }

        // GET: Garage
        public async Task<IActionResult> Index(string? search, string? sortOrder)
        {
            int occupiedSpots = _vehicleRepository.Count();
            int availableSpots = TotalParkingSpots - occupiedSpots;

            ViewBag.AvailableSpots = availableSpots;
            ViewBag.TotalSpots = TotalParkingSpots;


            ViewData["CurrentFilter"] = search;            
           
            ViewData[OverviewSorting.TYPE_SORT] = sortOrder == OverviewSorting.TYPE_ASC ? 
                OverviewSorting.TYPE_DESC : OverviewSorting.TYPE_ASC;
            ViewData[OverviewSorting.REG_SORT] = sortOrder == OverviewSorting.REG_ASC ?
                OverviewSorting.REG_DESC : OverviewSorting.REG_ASC;
            ViewData[OverviewSorting.ARRIVAL_SORT] = sortOrder == OverviewSorting.ARRIVAL_ASC ?
                OverviewSorting.ARRIVAL_DESC : OverviewSorting.ARRIVAL_ASC;
            ViewData[OverviewSorting.DURATION_SORT] = sortOrder == OverviewSorting.DURATION_ASC ?
                OverviewSorting.DURATION_DESC : OverviewSorting.DURATION_ASC;
            ViewData[OverviewSorting.BRAND_SORT] = sortOrder == OverviewSorting.BRAND_ASC ?
                OverviewSorting.BRAND_DESC : OverviewSorting.BRAND_ASC;
            ViewData[OverviewSorting.SPOT_SORT] = sortOrder == OverviewSorting.SPOT_ASC ?
                OverviewSorting.SPOT_DESC : OverviewSorting.SPOT_ASC;

            var query = _vehicleRepository.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim();

                var sReg = s.Replace(" ", "").ToUpper();

                query = query.Where(v =>
                    v.RegNumber.Contains(sReg) ||
                    v.Brand.Contains(s) ||
                    v.Model.Contains(s) ||
                    v.Color.Contains(s)
                );
            }

            IEnumerable<VehicleViewModel> viewModels = await query
                .Select(v => new VehicleViewModel {
                    Id = v.Id,
                    VehicleType = v.VehicleType,
                    RegNumber = v.RegNumber,
                    Brand = v.Brand,
                    ArrivalTime = v.ArrivalTime,
                    ParkingSpot = v.ParkingSpot
                })
                .ToListAsync();

            viewModels = sortOrder switch {
                OverviewSorting.TYPE_ASC => viewModels.OrderBy(v => v.VehicleType.ToString()),
                OverviewSorting.TYPE_DESC => viewModels.OrderByDescending(v => v.VehicleType.ToString()),

                OverviewSorting.REG_ASC => viewModels.OrderBy(v => v.RegNumber),
                OverviewSorting.REG_DESC => viewModels.OrderByDescending(v => v.RegNumber),

                OverviewSorting.ARRIVAL_ASC => viewModels.OrderBy(v => v.ArrivalTime),
                OverviewSorting.ARRIVAL_DESC => viewModels.OrderByDescending(v => v.ArrivalTime),

                // duration: shortest first => newest arrival first
                OverviewSorting.DURATION_ASC => viewModels.OrderByDescending(v => v.ArrivalTime),
                // duration_desc: longest first => oldest arrival first
                OverviewSorting.DURATION_DESC => viewModels.OrderBy(v => v.ArrivalTime),

                OverviewSorting.BRAND_ASC => viewModels.OrderBy(v => v.Brand),
                OverviewSorting.BRAND_DESC => viewModels.OrderByDescending(v => v.Brand),

                OverviewSorting.SPOT_ASC => viewModels.OrderBy(v => v.ParkingSpot),
                OverviewSorting.SPOT_DESC => viewModels.OrderByDescending(v => v.ParkingSpot),

                _ => viewModels.OrderBy(v => v.RegNumber),
            };            

            foreach (var vm in viewModels)
                vm.UpdateParkDuration();

            return View(viewModels);
        }


        // GET: Garage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var vehicle = await _vehicleRepository
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Garage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Garage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegNumber,VehicleType,Color,Brand,Model,NumberOfWheels")] Vehicle vehicle)
        {
            var usedSpots = _vehicleRepository.Vehicles.Select(v => v.ParkingSpot).ToList();
            var freeSpot = Enumerable.Range(1, TotalParkingSpots)
                         .Except(usedSpots)
                         .FirstOrDefault();
            if (freeSpot == 0)
            {
                ModelState.AddModelError("", "Sorry, the garage is full!");
                return View(vehicle);
            }

            vehicle.ParkingSpot = freeSpot;

            int occupiedSpots = await _vehicleRepository.CountAsync();
            if (occupiedSpots >= TotalParkingSpots)
            {
                ModelState.AddModelError("", "Garage is full. No available parking spots.");
            }

            var reg = vehicle.RegNumber?.Trim().Replace(" ", "").ToUpper();

            if (string.IsNullOrWhiteSpace(reg))
            {
                ModelState.AddModelError(nameof(vehicle.RegNumber), "Registration number is required.");
            }
            else if (reg.Length < 2)
            {
               ModelState.AddModelError(nameof(vehicle.RegNumber), "Registration number must be at least 2 characters long.");
            }
            else if (reg.Length > 7)
            {
               ModelState.AddModelError(nameof(vehicle.RegNumber), "Registration number cannot exceed 7 characters.");
            }
            else
            {
                vehicle.RegNumber = reg;

                bool exists = await _vehicleRepository.AnyAsync(v => v.RegNumber == reg);
                if (exists)
                {
                    ModelState.AddModelError(nameof(vehicle.RegNumber), "This registration number already exists.");
                }
            }

            if (ModelState.IsValid)
            {
                vehicle.ArrivalTime = DateTime.Now;
                await _vehicleRepository.Add(vehicle);
                TempData["Success"] = $"Vehicle checked in successfully. Parking Spot: {vehicle.ParkingSpot}";
                return RedirectToAction(nameof(Index));
            }

            return View(vehicle);
        }

        // GET: Garage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleRepository.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle); 
        }

        // POST: Garage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }
            
            var reg = vehicle.RegNumber?.Trim().Replace(" ", "").ToUpper();

            if (string.IsNullOrWhiteSpace(reg))
            {
                ModelState.AddModelError(nameof(vehicle.RegNumber), "Registration number is required.");
            }
            else if (reg.Length < 2)
            {
                ModelState.AddModelError(nameof(vehicle.RegNumber), "Registration number must be at least 2 characters long.");
            }
            else if (reg.Length > 7)
            {
                ModelState.AddModelError(nameof(vehicle.RegNumber), "Registration number cannot exceed 7 characters.");
            }
            else
            {
                vehicle.RegNumber = reg;
                bool exists = await _vehicleRepository.AnyAsync(v => v.RegNumber == reg && v.Id != vehicle.Id);
                if (exists)
                {
                    ModelState.AddModelError(nameof(vehicle.RegNumber), "This registration number already exists.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _vehicleRepository.Update(vehicle);
                    TempData["Success"] = "Vehicle updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }
        
        public async Task<IActionResult> Checkout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleRepository
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }            

            DeleteVehicleViewModel viewModel = new() {
                Id = vehicle.Id,
                RegNumber = vehicle.RegNumber,
                VehicleType = vehicle.VehicleType
            };

            return View(viewModel);
        }

        // POST: Garage/Delete/5
        [HttpPost, ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(DeleteVehicleViewModel viewModel)
        {
            var vehicle = await _vehicleRepository.FindAsync(viewModel.Id);
            if (vehicle is null)
            {
                return NotFound();
            }

            ReceiptViewModel receiptViewModel = new() {
                VehicleRegNumber = vehicle.RegNumber,
                VehicleType = vehicle.VehicleType,
                ArrivalTime = vehicle.ArrivalTime
            };

            await _vehicleRepository.Remove(vehicle);
            TempData["Success"] = "Vehicle checked out successfully.";

            // If the user wants a receipt
            if (viewModel.WantReceipt)             
                return RedirectToAction(nameof(Receipt), receiptViewModel);
            else 
                return RedirectToAction(nameof(Index));
        }

        private async Task<bool> VehicleExists(int id)
        {
            return await _vehicleRepository.AnyAsync(e => e.Id == id);
        }

        public IActionResult Receipt(ReceiptViewModel viewModel)
        {
            return View(viewModel);
        }
        public async Task<IActionResult> Statistics()
        {
            var vehicles = await _vehicleRepository.ToListAsync();
            GarageStatisticsViewModel stats = new GarageStatisticsViewModel
            {
                TotalVehicles = vehicles.Count,
                TotalWheels = vehicles.Sum(v => v.NumberOfWheels),
                VehiclesByType = vehicles
                .GroupBy(v => v.VehicleType)
                .ToDictionary(g => g.Key, g => g.Count()),
                EstimatedRevenue = vehicles.Sum(v =>
                (DateTime.Now - v.ArrivalTime).TotalHours * 30)
            };
            
            return View(stats);
        }
    }
}
