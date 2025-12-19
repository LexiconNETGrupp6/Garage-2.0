using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;
using Garage_2._0.Data;
using Garage_2._0.Models.ViewModels;
using Garage_2._0.ConstantStrings;

namespace Garage_2._0.Controllers
{
    public class GarageController : Controller
    {
        private readonly GarageContext _context;

        public GarageController(GarageContext context)
        {
            _context = context;
        }

        // GET: Garage
        public async Task<IActionResult> Index(string? search, string? sortOrder)
        {
            ViewData["CurrentFilter"] = search;

            var query = _context.Vehicle.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(v => v.RegNumber.Contains(search));
            }            

            IEnumerable<VehicleViewModel> viewModels = query.Select(v => new VehicleViewModel {
                Id = v.Id,
                VehicleType = v.VehicleType,
                RegNumber = v.RegNumber,
                ArrivalTime = v.ArrivalTime,
            });

            if (!string.IsNullOrWhiteSpace(sortOrder)) {
                viewModels = SortVehicles(sortOrder, viewModels);
            }

            return View(viewModels);
        }

        public IEnumerable<VehicleViewModel> SortVehicles(string sortOrder, IEnumerable<VehicleViewModel> vehicles)
        {
            // Flips a stored bool to switch between Ascending and Descening order
            VehicleViewModel.VehicleSortCategories[sortOrder] = !VehicleViewModel.VehicleSortCategories[sortOrder];

            Func<VehicleViewModel, string> condition;
            var dateNow = DateTime.Now;
            switch (sortOrder) {
                case VehicleViewModelSortingCategories.VehicleType:
                    condition = (v => v.VehicleType.ToString());
                    break;
                case VehicleViewModelSortingCategories.RegNumber:
                    condition = (v => v.RegNumber);
                    break;
                case VehicleViewModelSortingCategories.ArrivalTime:
                    condition = (v => v.ArrivalTime.ToString());
                    break;
                case VehicleViewModelSortingCategories.Duration:
                    // Doesn't use UpdateParkedDuration() or Parkduration because each vehicle would have
                    // a different baseline since DateTime.Now keeps changing during the loop.
                    condition = (v => dateNow.Subtract(v.ArrivalTime).ToString());
                    break;
                default:
                    return vehicles;
            }

            return VehicleViewModel.Sort(vehicles, condition, VehicleViewModel.VehicleSortCategories[sortOrder]);
        }

        // GET: Garage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
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
        public async Task<IActionResult> Create([Bind("Id,RegNumber,VehicleType,Color,Brand,Model,NumberOfWheels")] Vehicle vehicle)
        {
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

                bool exists = await _context.Vehicle.AnyAsync(v => v.RegNumber == reg);
                if (exists)
                {
                    ModelState.AddModelError(nameof(vehicle.RegNumber), "This registration number already exists.");
                }
            }

            if (ModelState.IsValid)
            {
                vehicle.ArrivalTime = DateTime.Now;
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Vehicle checked in successfully.";
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

            var vehicle = await _context.Vehicle.FindAsync(id);
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
                bool exists = await _context.Vehicle.AnyAsync(v => v.RegNumber == reg && v.Id != vehicle.Id);
                if (exists)
                {
                    ModelState.AddModelError(nameof(vehicle.RegNumber), "This registration number already exists.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Vehicle updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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

            var vehicle = await _context.Vehicle
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
            var vehicle = await _context.Vehicle.FindAsync(viewModel.Id);
            if (vehicle is null)
            {
                return NotFound();
            }

            ReceiptViewModel receiptViewModel = new() {
                VehicleRegNumber = vehicle.RegNumber,
                VehicleType = vehicle.VehicleType,
                ArrivalTime = vehicle.ArrivalTime
            };

            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Vehicle checked out successfully.";

            // If the user wants a receipt
            if (viewModel.WantReceipt)             
                return RedirectToAction(nameof(Receipt), receiptViewModel);            
            else 
                return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

        public IActionResult Receipt(ReceiptViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
