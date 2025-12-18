using Garage_2._0.Data;
using Garage_2._0.Models;
using Garage_2._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(string? search)
        {

            //await Seed();


            ViewData["CurrentFilter"] = search;

            var query = _context.Vehicle.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(v => v.RegNumber.Contains(search));
            }

            return View(await query.ToListAsync());
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
            var reg = vehicle.RegNumber?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(reg))
            {
                ModelState.AddModelError(nameof(vehicle.RegNumber), "Registration number is required.");
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,RegNumber,VehicleType,Color,Brand,Model,NumberOfWheels")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
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

        private async Task Seed()
        {
            _context.Vehicle.Add(new Vehicle { RegNumber = "ABC123", VehicleType = VehicleType.Car, Color = "Red", Brand = "Volvo", Model = "V70", NumberOfWheels = 4 });
            _context.Vehicle.Add(new Vehicle { RegNumber = "LGH436", VehicleType = VehicleType.Boat, Color = "Yellow", Brand = "East Marine", Model = "Viking Line", NumberOfWheels = 0 });
            _context.Vehicle.Add(new Vehicle { RegNumber = "AHC745", VehicleType = VehicleType.Bus, Color = "Red", Brand = "Volvo", Model = "V7900", NumberOfWheels = 8 });
            _context.Vehicle.Add(new Vehicle { RegNumber = "KAK156", VehicleType = VehicleType.Car, Color = "White", Brand = "Teesla", Model = "X", NumberOfWheels = 4 });
            _context.Vehicle.Add(new Vehicle { RegNumber = "IKA71U", VehicleType = VehicleType.Truck, Color = "Blue", Brand = "Scania", Model = "G-series", NumberOfWheels = 6 });
            _context.Vehicle.Add(new Vehicle { RegNumber = "ÅJAUIV", VehicleType = VehicleType.Motorcycle, Color = "Green", Brand = "Mercedez", Model = "L420", NumberOfWheels = 2 });
            await _context.SaveChangesAsync();
        }
    }
}
