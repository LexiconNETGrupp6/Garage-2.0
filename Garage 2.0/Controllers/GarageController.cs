using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Models;
using Garage_2._0.Data;
using Garage_2._0.Models.ViewModels;

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

            if (!string.IsNullOrWhiteSpace(sortOrder)) {
                query = SortVehicles(sortOrder, query);
            }

            IEnumerable<VehicleViewModel> viewModels = query.Select(v => new VehicleViewModel {
                Id = v.Id,
                VehicleType = v.VehicleType,
                RegNumber = v.RegNumber,
                ArrivalTime = v.ArrivalTime,
            });

            return View(viewModels);
        }

        public IQueryable<Vehicle> SortVehicles(string sordOrder, IQueryable<Vehicle> vehicles)
        {
            var date = DateTime.Now;
            // Sort based on attribute
            switch (sordOrder) {
                case "type":
                    vehicles = vehicles.OrderBy(v => v.VehicleType.ToString());
                    break;
                case "reg-number":
                    vehicles = vehicles.OrderBy(v => v.RegNumber);
                    break;
                case "arrival-time":
                    vehicles = vehicles.OrderBy(v => v.ArrivalTime.ToString());
                    break;
                case "duration":
                    // Attempts at getting this sorting to work.
                    // Keeps throwing error that it convert DateTime operations into SQL

                    //vehicles = vehicles.OrderBy(v => DateTime.Now - v.ArrivalTime);
                    //vehicles = from v in vehicles orderby DateTime.Now.Subtract(v.ArrivalTime) select v;
                    
                    //var durations = vehicles.Select(v => new { v.Id, Duration = (date - v.ArrivalTime).ToString() });
                    //vehicles = vehicles.OrderBy(v => durations.First(d => d.Id == v.Id).Duration);
                    break;
            }

            return vehicles;
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
