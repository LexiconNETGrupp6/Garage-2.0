using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Garage_2._0.Data;
using Garage_2._0.Models;

namespace Garage_2._0.Controllers
{
    public class ParkingGaragesController : Controller
    {
        private readonly GarageContext _context;

        public ParkingGaragesController(GarageContext context)
        {
            _context = context;
        }

        // GET: ParkingGarages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Garages.ToListAsync());
        }

        // GET: ParkingGarages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingGarage = await _context.Garages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingGarage == null)
            {
                return NotFound();
            }

            return View(parkingGarage);
        }

        // GET: ParkingGarages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ParkingGarages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Floors,LayoutJson")] ParkingGarage parkingGarage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parkingGarage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parkingGarage);
        }

        // GET: ParkingGarages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingGarage = await _context.Garages.FindAsync(id);
            if (parkingGarage == null)
            {
                return NotFound();
            }
            return View(parkingGarage);
        }

        // POST: ParkingGarages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Floors,LayoutJson")] ParkingGarage parkingGarage)
        {
            if (id != parkingGarage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parkingGarage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParkingGarageExists(parkingGarage.Id))
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
            return View(parkingGarage);
        }

        // GET: ParkingGarages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parkingGarage = await _context.Garages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parkingGarage == null)
            {
                return NotFound();
            }

            return View(parkingGarage);
        }

        // POST: ParkingGarages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parkingGarage = await _context.Garages.FindAsync(id);
            if (parkingGarage != null)
            {
                _context.Garages.Remove(parkingGarage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkingGarageExists(int id)
        {
            return _context.Garages.Any(e => e.Id == id);
        }
    }
}
