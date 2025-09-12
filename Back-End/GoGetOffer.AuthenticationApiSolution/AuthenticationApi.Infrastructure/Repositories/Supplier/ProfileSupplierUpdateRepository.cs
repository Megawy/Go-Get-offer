using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationApi.Infrastructure.Repositories.Supplier
{
    public class ProfileSupplierUpdateRepository(AuthenticationDbContext context) : IProfileSupplierUpdateRepository
    {
        private readonly AuthenticationDbContext _context = context;

        public async Task<Response<SuppilerProfileUpdate>> CreateAsync(SuppilerProfileUpdate entity)
        {
            await _context.SuppilerProfileUpdates.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Response<SuppilerProfileUpdate>.Success(entity, "Added successfully");
        }

        public async Task<Response<SuppilerProfileUpdate>> DeleteAsync(SuppilerProfileUpdate entity)
        {

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Response<SuppilerProfileUpdate>.Success(entity, $"{entity.Id} is deleted successfully");
        }

        public async Task<Response<SuppilerProfileUpdate>> FindByIdAsync(Guid id, bool includeDeleted = false)
        {
            var get = await _context.SuppilerProfileUpdates.FindAsync(id);
            if (get is null)
                return Response<SuppilerProfileUpdate>.Failure("not found.");

            return Response<SuppilerProfileUpdate>.Success(get);
        }

        public async Task<Response<SuppilerProfileUpdate>> FindUserNoTracking(Guid id)
        {
            var get = await _context.SuppilerProfileUpdates
                    .AsNoTracking()
                     .FirstOrDefaultAsync(u => u.Id == id);

            if (get == null)
            {
                return Response<SuppilerProfileUpdate>.Failure("User not found.");
            }

            return Response<SuppilerProfileUpdate>.Success(get);
        }

        public async Task<Response<IEnumerable<SuppilerProfileUpdate>>> GetAllAsync(bool includeDeleted = false)
        {
            var get = await _context.SuppilerProfileUpdates.AsNoTracking().ToListAsync();
            if (get is null)
                return Response<IEnumerable<SuppilerProfileUpdate>>.Failure($"NO Supplier in DB");
            return Response<IEnumerable<SuppilerProfileUpdate>>.Success(get, "Successify Get Supplier");
        }

        public async Task<Response<SuppilerProfileUpdate>> GetByAsync(Expression<Func<SuppilerProfileUpdate, bool>> predicate, bool includeDeleted = false)
        {
            var query = _context.SuppilerProfileUpdates.AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            var res = await query.FirstOrDefaultAsync(predicate);

            return Response<SuppilerProfileUpdate>.Success(res);
        }

        public async Task<Response<SuppilerProfileUpdate>> GetLastPendingRequestByUserId(Guid id)
        {
            var res = await _context.SuppilerProfileUpdates
                .Where(x => x.SupplierProfilesId == id)
                .OrderByDescending(x => x.RequestedAt)
                .FirstOrDefaultAsync();
            return Response<SuppilerProfileUpdate>.Success(res);
        }

        public async Task<Response<SuppilerProfileUpdate>> UpdateAsync(SuppilerProfileUpdate entity)
        {
            _context.SuppilerProfileUpdates.Update(entity);
            await _context.SaveChangesAsync();
            return Response<SuppilerProfileUpdate>.Success("updated successfully");
        }
    }
}
