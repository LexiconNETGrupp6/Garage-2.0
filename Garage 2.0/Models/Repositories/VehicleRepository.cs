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
            var a = await _context.Vehicle.ToListAsync();
        }

        public List<Vehicle> ToList()
            => _context.Vehicle.ToList();

        public async Task<List<Vehicle>> ToListAsync()
            => await _context.Vehicle.ToListAsync();

        public async Task<bool> AnyAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default)
            => await _context.Vehicle.AnyAsync(predicate, token);

        public async Task<Vehicle?> FirstOrDefaultAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default)
            => await _context.Vehicle.FirstOrDefaultAsync(predicate, token);

        public async Task<Vehicle?> FindAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default)
            => await _context.Vehicle.FirstOrDefaultAsync(predicate, token);

        public async Task<Vehicle?> FindAsync(params object?[]? keyValues)
            => await _context.Vehicle.FindAsync(keyValues);

        public IQueryable<Vehicle> AsNoTracking()
            => _context.Vehicle.AsNoTracking();

        public int Count() 
            => _context.Vehicle.Count();

        public int Count(Func<Vehicle, bool> predicate) 
            => _context.Vehicle.Count(predicate);
        public async Task<int> CountAsync() 
            => await _context.Vehicle.CountAsync();

        public async Task<int> CountAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default)
            => await _context.Vehicle.CountAsync(predicate, token);
    }
}
