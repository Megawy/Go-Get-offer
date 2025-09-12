using AuthenticationApi.Domain.Entites.Supplier;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories.Interfaces.Supplier;
using GoGetOffer.SharedLibrarySolution.DTOs.Supplier.Profile;
using GoGetOffer.SharedLibrarySolution.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationApi.Infrastructure.Repositories.Supplier
{
    public class SupplierBranchRepository : ISupplierBranchRepository
    {
        private readonly AuthenticationDbContext _context;
        public SupplierBranchRepository(AuthenticationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<SupplierBranch>> CreateAsync(SupplierBranch entity)
        {

            await _context.SupplierBranches.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierBranch>.Success(entity, "Added successfully");
        }

        public async Task<Response<SupplierBranch>> DeleteAsync(SupplierBranch entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierBranch>.Success(entity, $"{entity.Id} is deleted successfully");
        }

        public async Task<Response<SupplierBranch>> FindByIdAsync(Guid id, bool includeDeleted = false)
        {
            var get = await _context.SupplierBranches.FindAsync(id);
            if (get is null)
                return Response<SupplierBranch>.Failure("not found.");

            return Response<SupplierBranch>.Success(get);
        }

        public async Task<Response<SupplierBranch>> FindUserNoTracking(Guid id)
        {
            var get = await _context.SupplierBranches
                   .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

            if (get == null)
            {
                return Response<SupplierBranch>.Failure("User not found.");
            }

            return Response<SupplierBranch>.Success(get);
        }

        public async Task<Response<IEnumerable<SupplierBranch>>> GetAllAsync(bool includeDeleted = false)
        {

            var get = await _context.SupplierBranches
                .AsNoTracking()
                .ToListAsync();

            if (get is null)
                return Response<IEnumerable<SupplierBranch>>.Failure($"NO Supplier in DB");
            return Response<IEnumerable<SupplierBranch>>.Success(get, "Successify Get Supplier");
        }

        public async Task<Response<IEnumerable<SupplierBranch>>> GetAllBranchByUserCode(CodeSupplierDTO dto)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Code == dto.code);

            if (supplier is null)
                return Response<IEnumerable<SupplierBranch>>.Failure("Supplier not found.");

            var branches = await _context.SupplierBranches.AsNoTracking()
                .Where(b => b.SupplierProfilesId == supplier.Id)
                .ToListAsync();

            if (branches is null)
                return Response<IEnumerable<SupplierBranch>>.Failure($"NO Brances in DB");

            return Response<IEnumerable<SupplierBranch>>.Success(branches, "Successify Get Brances");
        }

        public async Task<Response<IEnumerable<SupplierBranch>>> GetAllBranchByUserId(Guid id)
        {

            var get = await _context.SupplierBranches
                .AsNoTracking()
                .Where(b => b.SupplierProfilesId == id)
                .ToListAsync();
            if (get is null)
                return Response<IEnumerable<SupplierBranch>>.Failure($"NO Brances in DB");

            return Response<IEnumerable<SupplierBranch>>.Success(get, "Successify Get Brances");
        }

        public async Task<Response<SupplierBranch>> GetByAsync(Expression<Func<SupplierBranch, bool>> predicate, bool includeDeleted = false)
        {
            var result = await
                _context.SupplierBranches.Where(predicate).FirstOrDefaultAsync()!;

            return Response<SupplierBranch>.Success(result);
        }

        public async Task<Response<SupplierBranch>> UpdateAsync(SupplierBranch entity)
        {
            _context.SupplierBranches.Update(entity);
            await _context.SaveChangesAsync();
            return Response<SupplierBranch>.Success(entity, "updated successfully");
        }
    }
}
