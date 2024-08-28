namespace LibraryManagementSystem.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _dbContext;

        public Repository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(T author)
        {
            await _dbContext.AddAsync(author);
            SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
