using LibraryManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : ModelBase
    {
        private ApplicationDbContext _dbContext;
        private DbSet<T> _dataSet;

        public Repository(ApplicationDbContext context)
        {
            _dbContext = context;
            _dataSet = _dbContext.Set<T>();
        }

        public async Task<T?> AddAsync(T entity,
            Expression<Func<T, bool>>? duplicateCheck = null)
        {
            if (duplicateCheck == null)
            {
                try
                {
                    return await AddAsync(entity);
                }
                catch
                {
                    throw new InvalidOperationException("Cannot add item. Check if duplicate.");
                }
            }
            else
            {
                var duplicateObject = await _dataSet.FirstOrDefaultAsync(duplicateCheck);
                if (duplicateObject != null)
                {
                    return null;
                }

                return await AddAsync(entity);
            }
        }

        private async Task<T> AddAsync(T entity)
        {
            var entry = await _dbContext.AddAsync(entity);
            await SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter == null)
            {
                return await _dataSet.ToListAsync();
            }

            return await _dataSet.Where(filter).ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
        {
            var result = await _dataSet.FirstOrDefaultAsync(filter);
            return result as T;
        }

        public async Task<T> RemoveAsync(T entity)
        {
            var result = await _dataSet.FindAsync(entity.Id);
            if (result == null)
                throw new InvalidOperationException();

            _dataSet.Remove(result);
            await SaveChangesAsync();
            return result;
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            if (id != entity.Id)
                throw new InvalidOperationException();

            var existingEntity = await _dataSet.FindAsync(id);
            if(existingEntity == null)
                throw new InvalidOperationException();

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            await SaveChangesAsync();
            return existingEntity;
        }
 
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
