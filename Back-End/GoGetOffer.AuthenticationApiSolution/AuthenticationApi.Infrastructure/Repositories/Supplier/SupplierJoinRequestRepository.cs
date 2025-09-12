using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationApi.Infrastructure.Repositories.Supplier
{
    public class SupplierJoinRequestRepository(AuthenticationDbContext context) : ISupplierJoinRequestRepository
    {
        private readonly AuthenticationDbContext _context = context;

        public async Task<Response<SupplierJoinRequest>> CreateAsync(SupplierJoinRequest entity)
        {
            await _context.SupplierJoinRequests.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierJoinRequest>.Success(entity, "Added successfully");
        }

        public async Task<Response<SupplierJoinRequest>> DeleteAsync(SupplierJoinRequest entity)
        {

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierJoinRequest>.Success(entity, $"{entity.Id} is deleted successfully");
        }

        public async Task<Response<SupplierJoinRequest>> FindByIdAsync(Guid id, bool includeDeleted = false)
        {

            var get = await _context.SupplierJoinRequests.FindAsync(id);
            if (get is null)
                return Response<SupplierJoinRequest>.Failure("not found.");

            return Response<SupplierJoinRequest>.Success(get);
        }

        public async Task<Response<SupplierJoinRequest>> FindUserNoTracking(Guid id)
        {
            var get = await _context.SupplierJoinRequests
                   .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

            if (get == null)
            {
                return Response<SupplierJoinRequest>.Failure("User not found.");
            }

            return Response<SupplierJoinRequest>.Success(get);
        }

        public async Task<Response<IEnumerable<SupplierJoinRequest>>> GetAllAsync(bool includeDeleted = false)
        {
            var get = await _context.SupplierJoinRequests
                .AsNoTracking()
                .ToListAsync();

            if (get is null)
                return Response<IEnumerable<SupplierJoinRequest>>.Failure($"NO Supplier in DB");

            return Response<IEnumerable<SupplierJoinRequest>>.Success(get, "Successify Get Supplier Join Request");
        }

        public async Task<Response<SupplierJoinRequest>> GetByAsync(Expression<Func<SupplierJoinRequest, bool>> predicate, bool includeDeleted = false)
        {
            var query = _context.SupplierJoinRequests.AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            var res = await query.FirstOrDefaultAsync(predicate);

            return Response<SupplierJoinRequest>.Success(res);
        }

        public async Task<Response<SupplierJoinRequest>> GetLastPendingRequestByUserIdAsync(Guid id)
        {
            var res = await _context.SupplierJoinRequests
                 .Where(x => x.SupplierProfilesId == id)
                 .OrderByDescending(x => x.RequestedAt)
                 .FirstOrDefaultAsync();

            if (res is null)
                return Response<SupplierJoinRequest>.Failure("Not Found.");

            return Response<SupplierJoinRequest>.Success(res);
        }

        public async Task<Response<SupplierJoinRequest>> UpdateAsync(SupplierJoinRequest entity)
        {
            _context.SupplierJoinRequests.Update(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierJoinRequest>.Success("updated successfully");
        }
    }
}
