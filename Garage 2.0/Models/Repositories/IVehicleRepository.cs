using System.Linq.Expressions;

namespace Garage_2._0.Models.Repositories
{
    public interface IVehicleRepository
    {
        IEnumerable<Vehicle> Vehicles { get; }

        Task Add(Vehicle vehicle);
        Task Update(Vehicle vehicle);
        Task Remove(Vehicle vehicle);
        Task<bool> AnyAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default);
        Task<Vehicle?> FindAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default);
        Task<Vehicle?> FindAsync(params object?[]? keyValues);
        Task<Vehicle?> FirstOrDefaultAsync(Expression<Func<Vehicle, bool>> predicate, CancellationToken token = default);
    }
}