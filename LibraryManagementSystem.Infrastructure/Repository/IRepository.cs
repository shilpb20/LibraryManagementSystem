using System.Linq.Expressions;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T author);

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);

        Task SaveChangesAsync();
    }
}