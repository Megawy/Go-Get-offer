using AuthenticationApi.Domain.Entites.Auth;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationApi.Infrastructure.Repositories.Authentication
{
    public class RequestUserUpdateRepository(AuthenticationDbContext context) : IRequestUserUpdateRepository
    {
        private readonly AuthenticationDbContext _context = context;
        public async Task<Response<AuthenticationUserUpdateRequest>> CreateAsync(AuthenticationUserUpdateRequest entity)
        {

            await _context.AuthenticationUserUpdateRequests.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Response<AuthenticationUserUpdateRequest>.Success(entity, "Added successfully");
        }

        public async Task<Response<AuthenticationUserUpdateRequest>> DeleteAsync(AuthenticationUserUpdateRequest entity)
        {

            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Response<AuthenticationUserUpdateRequest>.Success(entity, $"{entity.Id} is canceled successfully");
        }

        public async Task<Response<AuthenticationUserUpdateRequest>> FindByIdAsync(Guid id, bool includeDeleted = false)
        {

            var query = _context.AuthenticationUserUpdateRequests.AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            var res = await query.FirstOrDefaultAsync(u => u.Id == id);

            if (res == null)
            {
                return Response<AuthenticationUserUpdateRequest>.Failure("not found.");
            }

            return Response<AuthenticationUserUpdateRequest>.Success(res);
        }

        public async Task<Response<AuthenticationUserUpdateRequest>> FindUserNoTracking(Guid id)
        {
            var user = await _context.AuthenticationUserUpdateRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return Response<AuthenticationUserUpdateRequest>.Failure("User not found.");
            }

            return Response<AuthenticationUserUpdateRequest>.Success(user);
        }

        public async Task<Response<IEnumerable<AuthenticationUserUpdateRequest>>> GetAllAsync(bool includeDeleted = false)
        {
            var query = _context.AuthenticationUserUpdateRequests.AsNoTracking().AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            var res = await query
                .Where(x => x.IsApproved == IsApproved.Pending)
                .OrderByDescending(x => x.RequestedAt)
                .ToListAsync();

            if (res.Count == 0)
            {
                return Response<IEnumerable<AuthenticationUserUpdateRequest>>.Failure("No found");
            }

            return Response<IEnumerable<AuthenticationUserUpdateRequest>>.Success(res, "retrieved successfully");
        }

        public async Task<Response<AuthenticationUserUpdateRequest>> GetByAsync(Expression<Func<AuthenticationUserUpdateRequest, bool>> predicate, bool includeDeleted = false)
        {

            var query = _context.AuthenticationUserUpdateRequests.AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            var res = await query.FirstOrDefaultAsync(predicate);

            return Response<AuthenticationUserUpdateRequest>.Success(res);
        }

        public async Task<Response<AuthenticationUserUpdateRequest>> GetLastPendingRequestByUserIdAsync(Guid id)
        {

            var res = await _context.AuthenticationUserUpdateRequests
                .Where(x => x.AuthenticationUserId == id)
                .OrderByDescending(x => x.RequestedAt)
                .FirstOrDefaultAsync();

            return Response<AuthenticationUserUpdateRequest>.Success(res);
        }

        public async Task<Response<AuthenticationUserUpdateRequest>> UpdateAsync(AuthenticationUserUpdateRequest entity)
        {
            _context.AuthenticationUserUpdateRequests.Update(entity);
            await _context.SaveChangesAsync();
            return Response<AuthenticationUserUpdateRequest>.Success("updated successfully");
        }
    }
}
