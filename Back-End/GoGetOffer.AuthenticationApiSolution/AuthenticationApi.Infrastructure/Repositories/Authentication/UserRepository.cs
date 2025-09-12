using AuthenticationApi.Domain.Entites.Auth;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Authentication;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationApi.Infrastructure.Repositories.Authentication
{
    public class UserRepository(AuthenticationDbContext context) : IUserRepository
    {
        private readonly AuthenticationDbContext _context = context;

        public async Task<Response<AuthenticationUser>> CreateAsync(AuthenticationUser entity)
        {
            await _context.AuthenticationUser.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Response<AuthenticationUser>.Success(entity, "User added successfully");

        }

        public async Task<Response<AuthenticationUser>> UpdateAsync(AuthenticationUser entity)
        {

            _context.AuthenticationUser.Update(entity);
            _context.Entry(entity).Property(x => x.PasswordHash).IsModified = false;
            await _context.SaveChangesAsync();
            return Response<AuthenticationUser>.Success(entity, "User updated successfully");

        }

        public async Task<Response<AuthenticationUser>> DeleteAsync(AuthenticationUser entity)
        {

            _context.AuthenticationUser.Update(entity);
            _context.Entry(entity).Property(x => x.PasswordHash).IsModified = false;
            await _context.SaveChangesAsync();

            return Response<AuthenticationUser>.Success("User soft-deleted successfully.");

        }

        public async Task<Response<AuthenticationUser>> FindByIdAsync(Guid id, bool includeDeleted = false)
        {

            var user = await GetUserQueryWithFilters(includeDeleted)
               .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return Response<AuthenticationUser>.Failure("User not found.");
            }

            return Response<AuthenticationUser>.Success(user);

        }

        public async Task<Response<IEnumerable<AuthenticationUser>>> GetAllAsync(bool includeDeleted = false)
        {
            var users = await GetUserQueryWithFilters(includeDeleted).AsNoTracking().ToListAsync();

            if (users.Count == 0)
            {
                return Response<IEnumerable<AuthenticationUser>>.Failure("No users found");
            }

            return Response<IEnumerable<AuthenticationUser>>.Success(users, "Users retrieved successfully");
        }

        public async Task<Response<AuthenticationUser>> GetByAsync(Expression<Func<AuthenticationUser, bool>> predicate, bool includeDeleted = false)
        {
            var user = await GetUserQueryWithFilters(includeDeleted)
                .FirstOrDefaultAsync(predicate);

            return Response<AuthenticationUser>.Success(user);
        }

        public async Task<Response<AuthenticationUser>> UpdatePassword(AuthenticationUser entity)
        {

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Response<AuthenticationUser>.Success("Password User updated successfully");
        }

        public async Task<Response<AuthenticationUser>> UpdateSpecificPropertiesAsync(Guid id, Action<AuthenticationUser> updateAction)
        {
            var user = await _context.AuthenticationUser.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return Response<AuthenticationUser>.Failure("User not found.");
            }

            updateAction(user);
            await _context.SaveChangesAsync();

            return Response<AuthenticationUser>.Success(user, "User updated successfully");
        }

        public async Task<bool> ExistsAsync(Guid id, bool includeDeleted = false)
        {

            return await GetUserQueryWithFilters(includeDeleted)
                .AnyAsync(u => u.Id == id);

        }

        private IQueryable<AuthenticationUser> GetUserQueryWithFilters(bool includeDeleted)
        {
            var query = _context.AuthenticationUser.AsQueryable();

            if (includeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }

            return query;
        }

        public async Task<Response<AuthenticationUser>> FindUserNoTracking(Guid id)
        {
            var user = await _context.AuthenticationUser
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return Response<AuthenticationUser>.Failure("User not found.");
            }

            return Response<AuthenticationUser>.Success(user);
        }
    }
}
