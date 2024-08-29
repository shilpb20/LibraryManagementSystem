using LibraryManagementSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : ModelBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dataSet;

        public Repository(ApplicationDbContext context)
        {
            _dbContext = context;
            _dataSet = _dbContext.Set<T>();
        }

        public async Task<T?> AddAsync(T entity,
            Expression<Func<T, bool>>? duplicateCheckBeforeAdd = null)
        {
            CheckNullEntity(entity);

            if (duplicateCheckBeforeAdd != null)
            {
                var duplicateObject = await _dataSet.FirstOrDefaultAsync(duplicateCheckBeforeAdd);
                if (duplicateObject != null)
                {
                    return null;
                }
            }

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
            CheckNullEntity(entity);

            var result = await _dataSet.FindAsync(entity.Id);
            if (result == null)
                throw new InvalidOperationException($"Entity with Id: {entity.Id} not found");

            _dataSet.Remove(result);
            await SaveChangesAsync();
            return result;
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            CheckNullEntity(entity);

            if (id != entity.Id)
                throw new InvalidOperationException("Entity Id mismatch during update.");

            var existingEntity = await _dataSet.FindAsync(id);
            if(existingEntity == null)
                throw new InvalidOperationException($"Entity with Id : {entity.Id} not found");

            entity.UpdateLastModifiedTime();

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
            await SaveChangesAsync();
            return existingEntity;
        }
 
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        private void CheckNullEntity(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
            }
        }
    }
}
