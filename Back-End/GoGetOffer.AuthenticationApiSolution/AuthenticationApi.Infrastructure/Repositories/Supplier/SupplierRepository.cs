using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationApi.Infrastructure.Repositories.Supplier
{
    public class SupplierRepository(AuthenticationDbContext context) : ISupplierRepository
    {
        private readonly AuthenticationDbContext _context = context;

        public async Task<Response<SupplierProfile>> CreateAsync(SupplierProfile entity)
        {
            await _context.Suppliers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierProfile>.Success(entity, "Added successfully");
        }

        public async Task<Response<SupplierProfile>> DeleteAsync(SupplierProfile entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierProfile>.Success(entity, $"{entity.Id} is deleted successfully");
        }

        public async Task<Response<SupplierProfile>> FindByIdAsync(Guid id, bool includeDeleted = false)
        {
            var get = await _context.Suppliers.FindAsync(id);
            if (get is null)
                return Response<SupplierProfile>.Failure("not found.");
            return Response<SupplierProfile>.Success(get);
        }

        public async Task<Response<SupplierProfile>> FindUserNoTracking(Guid id)
        {
            var get = await _context.Suppliers
                   .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

            if (get == null)
            {
                return Response<SupplierProfile>.Failure("not found.");
            }

            return Response<SupplierProfile>.Success(get);
        }

        public async Task<Response<IEnumerable<SupplierProfile>>> GetAllAsync(bool includeDeleted = false)
        {
            var get = await _context.Suppliers
                .AsNoTracking()
                .ToListAsync();

            if (get is null)
                return Response<IEnumerable<SupplierProfile>>.Failure($"NO Supplier in DB");

            return Response<IEnumerable<SupplierProfile>>.Success(get, "Successify Get Supplier");
        }

        public async Task<Response<SupplierProfile>> GetByAsync(Expression<Func<SupplierProfile, bool>> predicate, bool includeDeleted = false)
        {
            var query = _context.Suppliers.AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            var res = await query.FirstOrDefaultAsync(predicate);

            return Response<SupplierProfile>.Success(res);
        }

        public async Task<Response<SupplierProfile>> UpdateAsync(SupplierProfile entity)
        {
            _context.Suppliers.Update(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierProfile>.Success("updated successfully");
        }
    }
}
