using FluentAssertions;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace LibraryManagementSystem.Tests.Repository
{
    public class AuthorRepositoryTests : TestBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Author> _authorRepository;

        public AuthorRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagementSystem_Api_Test")
                .Options;

            _context = new ApplicationDbContext(dbContextOptions);
            _authorRepository = new Repository<Author>(_context);
        }

        [Fact]
        public async Task AddAuthor_ValidObject_AddsSuccessfully()
        {
            //Arrange
            //Act
            var author = new Author()
            {
                Id = 1,
                Name = "Simon Sinek"
            };

            await _authorRepository.AddAsync(author);

            //Assert
            Author addedObject = await _context.Authors.FirstOrDefaultAsync(x => x.Name == author.Name);

            addedObject.Should().NotBeNull();
            addedObject?.Id.Should().Be(author.Id);
            addedObject?.Name.Should().Be(author.Name);

        }
    }
}
