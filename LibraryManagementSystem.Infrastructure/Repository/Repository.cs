using LibraryManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _dbContext;
        private DbSet<T> _dataSet;

        public Repository(ApplicationDbContext context)
        {
            _dbContext = context;
            _dataSet = _dbContext.Set<T>();
        }

        public async Task AddAsync(T author)
        {
            await _dbContext.AddAsync(author);
            await SaveChangesAsync();
        }

        public async Task<T> GetAsync(Expression<Func<object, bool>> filter)
        {
            var result = await _dataSet.FirstOrDefaultAsync(filter);
            return result as T;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
