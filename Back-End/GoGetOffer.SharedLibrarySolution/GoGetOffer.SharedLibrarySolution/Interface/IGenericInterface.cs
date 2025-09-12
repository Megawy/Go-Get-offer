using GoGetOffer.SharedLibrarySolution.Responses;
using System.Linq.Expressions;
namespace GoGetOffer.SharedLibrarySolution.Interface
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Response<T>> CreateAsync(T entity);
        Task<Response<T>> UpdateAsync(T entity);
        Task<Response<T>> DeleteAsync(T entity);
        Task<Response<IEnumerable<T>>> GetAllAsync(bool includeDeleted = false);
        Task<Response<T>> FindByIdAsync(Guid id, bool includeDeleted = false);
        Task<Response<T>> FindUserNoTracking(Guid id);
        Task<Response<T>> GetByAsync(Expression<Func<T, bool>> predicate, bool includeDeleted = false);
    }
}
