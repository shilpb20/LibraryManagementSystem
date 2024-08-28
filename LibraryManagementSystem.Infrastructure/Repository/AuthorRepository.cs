using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Infrastructure.Repository
{
    public class AuthorRepository
    {
        private ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddAsync(Author author)
        {
            await _dbContext.AddAsync(author);
            await _dbContext.SaveChangesAsync();
        }
    }
}
