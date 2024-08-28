namespace LibraryManagementSystem.Infrastructure.Repository
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T author);
        Task SaveChangesAsync();
    }
}