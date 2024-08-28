using LibraryManagementSystem.Core.Models;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T author);
        Task<T> GetAsync(Expression<Func<object, bool>> filter);

        Task SaveChangesAsync();
    }
}