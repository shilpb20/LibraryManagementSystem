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

        private Author CreateAuthor()
        {
            return new Author()
            {
                Id = 1,
                Name = "Simon Sinek"
            };
        }

        [Fact]
        public async Task AddAsync_ValidObject_AddsSuccessfully()
        {
            //Arrange
            //Act
            Author author = CreateAuthor();

            await _authorRepository.AddAsync(author);

            //Assert
            Author? addedObject = await _context.Authors.FirstOrDefaultAsync(x => x.Name == author.Name);

            addedObject.Should().NotBeNull();
            addedObject?.Id.Should().Be(author.Id);
            addedObject?.Name.Should().Be(author.Name);
        }



        [Fact]
        public async Task GetAsync_MatchingData_ReturnesMatchingObject()
        {
            //Arrange
            //Act
            Author author = CreateAuthor();

            await _authorRepository.AddAsync(author);

            //Assert
            var addedObject = await _authorRepository.GetAsync(x => ((Author)x).Name == author.Name);

            addedObject.Should().NotBeNull();
            addedObject?.Id.Should().Be(author.Id);
            addedObject?.Name.Should().Be(author.Name);
        }

        [Fact]
        public async Task GetAsync_NoCondition_ThrowsArgumentNullException()
        {
            //Arrange
            //Act
            Author author = CreateAuthor();

            await _authorRepository.AddAsync(author);

            //Assert
            Func<Task> taskResult = async () => await _authorRepository.GetAsync(null);
            await taskResult.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetAsync_NoMatchingData_ReturnsNull()
        {
            //Arrange
            //Act
            Author author = CreateAuthor();

            await _authorRepository.AddAsync(author);

            //Assert
            var addedObject = await _authorRepository.GetAsync(x => ((Author)x).Name == "abc");

            addedObject.Should().BeNull();
        }
    }
}
