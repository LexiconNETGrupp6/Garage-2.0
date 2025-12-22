using Garage_2._0.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Garage_2._0.Models.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly GarageContext _context;

        public IEnumerable<Vehicle> Vehicles => _context.Vehicle;

        public VehicleRepository(GarageContext context)
        {
            _context = context;
        }

        public async Task Add(Vehicle vehicle)
        {
            _context.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Vehicle vehicle)
        {
            _context.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(Vehicle vehicle)
        {
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default)
            => await _context.Vehicle.AnyAsync(predicate, token);

        public async Task<Vehicle?> FirstOrDefaultAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default) 
            => await _context.Vehicle.FirstOrDefaultAsync(predicate, token);

        public async Task<Vehicle?> FindAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default) 
            => await _context.Vehicle.FirstOrDefaultAsync(predicate, token);

        public async Task<Vehicle?> FindAsync(params object?[]? keyValues) 
            => await _context.Vehicle.FindAsync(keyValues);
    }
}
