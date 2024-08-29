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
        #region fields-and-properties

        private readonly ApplicationDbContext _context;
        private readonly IRepository<Author> _authorRepository;

        #endregion

        #region constructors-and-initialisors

        public AuthorRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagementSystem_Api_Test")
                .Options;

            _context = new ApplicationDbContext(dbContextOptions);
            _authorRepository = new Repository<Author>(_context);
        }

        #endregion

        #region utility-methods

        private async Task<List<Author>> AddAuthorsToRepository()
        {
            var authors = AuthorList;
            foreach (var author in authors)
            {
                var matchingEntities = await _authorRepository.GetAsync(x => x.Name == author.Name);
                if (matchingEntities != null)
                    continue;

                await _authorRepository.AddAsync(author);
            }

            return authors;
        }

        private static List<Author> AuthorList = new List<Author>()
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
                }
        };

        #endregion

        #region tests

        #region add-async

        [Fact]
        public async Task AddAsync_ValidObject_NoCheck_AddsSuccessfully()
        {
            //Arrange
            //Act
            Author author = new Author()
            {
                Id = 7,
                Name = "James Clear"
            };

            var result = await _authorRepository.AddAsync(author);

            result.Should().NotBeNull();
            result?.Id.Should().Be(author.Id);
            result?.Name.Should().Be(author.Name);
        }

        [Fact]
        public async Task AddAsync_ValidObject_DuplicateCheck_AddsSuccessfully()
        {
            //Arrange
            //Act
            Author author = new Author()
            {
                Id = 8,
                Name = "Angela Duckworth"
            };

            var result = await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            //Assert
            Author? addedObject = await _context.Authors.FirstOrDefaultAsync(x => x.Name == author.Name);

            addedObject.Should().NotBeNull();
            addedObject?.Id.Should().Be(author.Id);
            addedObject?.Name.Should().Be(author.Name);
        }

        [Fact]
        public async Task AddAsync_DuplicateObject_DuplicateCheck_ReturnsNull()
        {
            //Arrange
            //Act
            Author author = new Author()
            {
                Id = 8,
                Name = "Angela Duckworth"
            };

            var result = await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_DuplicateObject_NoCheck_ThrowsException()
        {
            //Arrange
            //Act
            Author author = new Author()
            {
                Id = 8,
                Name = "Angela Duckworth"
            };

            await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            //Assert
           await FluentActions.Invoking(() => _authorRepository.AddAsync(author)).Should().ThrowAsync<InvalidOperationException>();

        }

        #endregion

        #region get-async


        [Fact]
        public async Task GetAsync_ValidFilter_ReturnesMatchingObject()
        {
            //Arrange
            //Act
            await AddAuthorsToRepository();
            var author = AuthorList.First();
            var addedObject = await _authorRepository.GetAsync(x => x.Name == author.Name);

            //Assert
            addedObject.Should().NotBeNull();
            addedObject?.Id.Should().Be(author.Id);
            addedObject?.Name.Should().Be(author.Name);
        }

        [Fact]
        public async Task GetAsync_NoFilter_ThrowsArgumentNullException()
        {
            //Arrange
            //Act
            Author author = AuthorList.First();

            //Assert
            Func<Task> taskResult = async () => await _authorRepository.GetAsync(null);
            await taskResult.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetAsync_ValidFilter_NoMatchingData_ReturnsNull()
        {
            //Arrange
            //Act
            Author author = AuthorList.First();

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
            List<Author> authors = await AddAuthorsToRepository();

            //Assert
            var allAuthors = await _authorRepository.GetAllAsync();

            allAuthors.Should().NotBeNull();
            allAuthors.Count().Should().BeGreaterThan(0);
            allAuthors.Count().Should().BeGreaterThanOrEqualTo(allAuthors.Count);

            int i = 0;
            foreach (var author in allAuthors)
            {
                _outputHelper.WriteLine($"Author {i + 1}");
                _outputHelper.WriteLine($"Id = {author.Id}");
                _outputHelper.WriteLine($"Name = {author.Name}");

                i++;
            }
        }


        [Fact]
        public async Task GetAllAsync_ValidFilter_ReturnsOnlyMatchingResults()
        {
            //Arrange
            //Act
            List<Author> authors = await AddAuthorsToRepository();
            var matchingAuthors = authors.Where(x => x.Name.Contains('a')).ToList();

            //Assert
            var matchingAuthorList = await _authorRepository.GetAllAsync(x => x.Name.Contains('a'));

            matchingAuthorList.Count.Should().NotBe(authors.Count);
            matchingAuthorList.Should().NotBeNull();
            matchingAuthorList.Count().Should().BeGreaterThan(0);
            matchingAuthorList.Count().Should().BeGreaterThanOrEqualTo(matchingAuthors.Count);

            int i = 0;
            foreach (var author in matchingAuthorList)
            {
                _outputHelper.WriteLine($"Author {i + 1}");
                _outputHelper.WriteLine($"Id = {author.Id}");
                _outputHelper.WriteLine($"Name = {author.Name}");

                i++;
            }
        }

        #endregion

        #region delete-async

        [Fact]
        public async Task RemoveAsync_ValidObject_DeletesSuccessfully()
        {
            //Arrange
            await AddAuthorsToRepository();

            //Act
            Author author = new Author()
            {
                Id = 7,
                Name = "James Clear"
            };

            await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            var matchingObject = await _authorRepository.GetAsync(x => x.Name == author.Name);
            matchingObject.Should().NotBeNull();
            matchingObject?.Id.Should().Be(author.Id);
            matchingObject?.Name.Should().Be(author.Name);

            var result = await _authorRepository.RemoveAsync(author);
            result.Should().NotBeNull();
            result?.Id.Should().Be(author.Id);
            result?.Name.Should().Be(author.Name);

            var matchingObjectAfterRemoval = await _authorRepository.GetAsync(x => x.Name == author.Name);
            matchingObjectAfterRemoval.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_InvalidObject_ThrowsInvalidOperationException()
        {
            //Arrange
            await AddAuthorsToRepository();

            //Act
            Author author = new Author()
            {
                Id = 10,
                Name = "Uknown Author"
            };

            var matchingObject = await _authorRepository.GetAsync(x => x.Name == author.Name);
            matchingObject.Should().BeNull();

            await FluentActions.Invoking(() => _authorRepository.RemoveAsync(author))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        #endregion

        #region update-async

        [Fact]
        public async Task UpdateAsync_ValidObject_UpdatesSuccessfully()
        {
            //Arrange
            await AddAuthorsToRepository();

            //Act
            Author author = new Author()
            {
                Id = 7,
                Name = "James Clear"
            };

            Author updatedAuthor = new Author()
            {
                Id = 7,
                Name = "James Hunt"
            };

            await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            var matchingObject = await _authorRepository.GetAsync(x => x.Name == author.Name);
            matchingObject.Should().NotBeNull();
            matchingObject?.Id.Should().Be(author.Id);
            matchingObject?.Name.Should().Be(author.Name);

            var result = await _authorRepository.UpdateAsync(author.Id, updatedAuthor);
            result.Should().NotBeNull();
            result?.Id.Should().Be(updatedAuthor.Id);
            result?.Name.Should().Be(updatedAuthor.Name);
        }

        [Fact]
        public async Task UpdateAsync_NoModification_UpdatesSuccessfully()
        {
            //Arrange
            await AddAuthorsToRepository();

            //Act
            Author author = new Author()
            {
                Id = 7,
                Name = "James Clear"
            };

            Author updatedAuthor = new Author()
            {
                Id = 7,
                Name = "James Clear"
            };

            await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            var matchingObject = await _authorRepository.GetAsync(x => x.Name == author.Name);
            matchingObject.Should().NotBeNull();
            matchingObject?.Id.Should().Be(author.Id);
            matchingObject?.Name.Should().Be(author.Name);

            var result = await _authorRepository.UpdateAsync(author.Id, updatedAuthor);
            result.Should().NotBeNull();
            result?.Id.Should().Be(author.Id);
            result?.Name.Should().Be(author.Name);
        }

        [Fact]
        public async Task UpdateAsync_IdModification_ThrowsInvalidOperationException()
        {
            //Arrange
            await AddAuthorsToRepository();

            //Act
            Author author = new Author()
            {
                Id = 7,
                Name = "James Clear"
            };

            Author updatedAuthor = new Author()
            {
                Id = 8,
                Name = "James Clear"
            };

            await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            var matchingObject = await _authorRepository.GetAsync(x => x.Name == author.Name);
            matchingObject.Should().NotBeNull();
            matchingObject?.Id.Should().Be(author.Id);
            matchingObject?.Name.Should().Be(author.Name);

            await FluentActions.Invoking(() => _authorRepository.UpdateAsync(author.Id, updatedAuthor))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task UpdateAsync_InvalidObject_ThrowsInvalidOperationException()
        {
            //Arrange
            await AddAuthorsToRepository();

            //Act
            Author author = new Author()
            {
                Id = 10,
                Name = "Uknown Author"
            };

            var matchingObject = await _authorRepository.GetAsync(x => x.Name == author.Name);
            matchingObject.Should().BeNull();

            await FluentActions.Invoking(() => _authorRepository.UpdateAsync(author.Id, author))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        #endregion

        #endregion
    }
}
