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

        private List<Author> CreateAuthors()
        {
            return new List<Author>()
            {
                new Author()
                {
                    Id = 1,
                    Name = "Simon Sinek"
                },
                new Author()
                {
                    Id = 2,
                    Name = "Malcolm Gladwell"
                },
                new Author()
                {
                    Id = 3,
                    Name = "Daniel Pink"
                },
                new Author()
                {
                    Id = 4,
                    Name = "Brené Brown"
                },
                new Author()
                {
                    Id = 5,
                    Name = "Adam Grant"
                },
                new Author()
                {
                    Id = 6,
                    Name = "Jim Collins"
                },
            };
        }

        #region tests

        #region add-async

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

        #endregion

        #region get-async


        [Fact]
        public async Task GetAsync_ValidFilter_ReturnesMatchingObject()
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
        public async Task GetAsync_NoFilter_ThrowsArgumentNullException()
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
        public async Task GetAsync_ValidFilter_NoMatchingData_ReturnsNull()
        {
            //Arrange
            //Act
            Author author = CreateAuthor();

            await _authorRepository.AddAsync(author);

            //Assert
            var addedObject = await _authorRepository.GetAsync(x => x.Name == "abc");

            addedObject.Should().BeNull();
        }

        #endregion

        #region get-all-async

        [Fact]
        public async Task GetAllAsync_NoFilter_ReturnsAll()
        {
            //Arrange
            //Act
            var authors = CreateAuthors();

            foreach(var author in authors)
            {
                await _authorRepository.AddAsync(author);
            }

            //Assert
            var allAuthors = await _authorRepository.GetAllAsync();

            allAuthors.Should().NotBeNull();
            allAuthors.Count().Should().Be(authors.Count);

            int i = 0;
            foreach(var author in allAuthors)
            {
                _outputHelper.WriteLine($"Author {i+1}");
                _outputHelper.WriteLine($"Id = {author.Id}");
                _outputHelper.WriteLine($"Name = {author.Name}");

                author.Id.Should().Be(authors[i].Id);
                author.Name.Should().Be(authors[i].Name);

                i++;
            }
        }

        [Fact]
        public async Task GetAllAsync_ValidFilter_ReturnsOnlyMatchingResults()
        {
            //Arrange
            //Act
            var authors = CreateAuthors();
            foreach (var author in authors)
            {
                await _authorRepository.AddAsync(author);
            }

            var matchingAuthors = authors.Where(x => x.Name.Contains('a')).ToList();

            //Assert
            var matchingAuthorList = await _authorRepository.GetAllAsync(x => x.Name.Contains('a'));

            matchingAuthorList.Count.Should().NotBe(authors.Count);
            matchingAuthorList.Should().NotBeNull();
            matchingAuthorList.Count().Should().Be(matchingAuthors.Count());

            int i = 0;
            foreach (var author in matchingAuthorList)
            {
                _outputHelper.WriteLine($"Author {i + 1}");
                _outputHelper.WriteLine($"Id = {author.Id}");
                _outputHelper.WriteLine($"Name = {author.Name}");

                author.Id.Should().Be(matchingAuthors[i].Id);
                author.Name.Should().Be(matchingAuthors[i].Name);

                i++;
            }
        }

        #endregion

        #endregion
    }
}
