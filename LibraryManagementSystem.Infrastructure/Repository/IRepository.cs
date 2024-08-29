using System.Linq.Expressions;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T?> AddAsync(T entity, Expression<Func<T, bool>>? duplicateCheck = null);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);

        Task SaveChangesAsync();
    }
}