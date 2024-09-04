using FluentAssertions;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure;
using LibraryManagementSystem.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace LibraryManagementSystem.Tests.Repository
{
    public class RepositoryTests : TestBase
    {
        #region fields-and-properties

        private readonly List<Author> _authors;
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Author> _authorRepository;

        #endregion

        #region constructors-and-initialisors

        public RepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            Console.WriteLine("Setup: Initializing resources.");

            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "LibraryManagementSystem_Api_Test")
                .Options;

            _context = new ApplicationDbContext(dbContextOptions);
            _authorRepository = new Repository<Author>(_context);
             _context.Database.EnsureCreated();

            _authors = _context.Authors.ToList();
        }


        #endregion

        #region tests

        #region add-async

        [Fact]
        public async Task AddAsync_ValidObject_NoCheck_AddsSuccessfully()
        {
            //Arrange
            //Act
            Author author = GetDanBrown();

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
            Author author = GetAngelaDuckworth();

            var result = await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            //Assert
            Author? addedObject = await _context.Authors.FirstOrDefaultAsync(x => x.Name == author.Name);

            addedObject.Should().NotBeNull();
            addedObject?.Id.Should().Be(author.Id);
            addedObject?.Name.Should().Be(author.Name);
        }

        [Fact]
        public async Task AddAsync_DuplicateObject_WithDuplicateCheck_ReturnsNull()
        {
            //Arrange
            //Act
            Author author = GetAngelaDuckworth();

            await _authorRepository.AddAsync(author, x => x.Name == author.Name);
            var result = await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_DuplicateObject_NoCheck_ThrowsArgumentException()
        {
            //Arrange
            //Act
            Author author = GetAngelaDuckworth();

            await _authorRepository.AddAsync(author, x => x.Name == author.Name);

            //Assert
            await FluentActions.Invoking(() => _authorRepository.AddAsync(author)).Should().ThrowAsync<ArgumentException>();

        }

        [Fact]
        public async Task AddAsync_NullObject_ThrowsArgumentNullException()
        {
            //Arrange
            //Act
            //Assert
            await FluentActions.Invoking(() => _authorRepository.AddAsync(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        #endregion

        #region get-async


        [Fact]
        public async Task GetAsync_ValidFilter_ReturnesMatchingObject()
        {
            //Arrange
            //Act
            var author = _authors.First();
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
            Author author = _authors.First();

            //Assert
            Func<Task> taskResult = async () => await _authorRepository.GetAsync(null);
            await taskResult.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetAsync_ValidFilter_NoMatchingData_ReturnsNull()
        {
            //Arrange
            //Act
            Author author = _authors.First();

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
            //List<Author> authors = await AddAuthorsToRepository();

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
            //List<Author> authors = await AddAuthorsToRepository();

            List<Author> authors = await _context.Authors.ToListAsync();
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
            Author lastObject = _context.Authors.OrderByDescending(x => x.Id).First();

            lastObject.Should().NotBeNull();

            //Act
            var result = await _authorRepository.RemoveAsync(lastObject);

            //Assert
            result.Should().NotBeNull();

            Author newLastObject = _context.Authors.OrderByDescending(x => x.Id).First();
            lastObject.Id.Should().BeGreaterThan(newLastObject.Id);
        }

        [Fact]
        public async Task RemoveAsync_InvalidObject_ThrowsInvalidOperationException()
        {
            //Arrange
            var author = _context.Authors.OrderByDescending(x => x.Id).First();

            var invalidAuthor = new Author()
            {
                Id = author.Id + 1,
                Name = "Author not presennt"
            };

            //Act
            //Assert
            await FluentActions.Invoking(() => _authorRepository.RemoveAsync(invalidAuthor))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task RemoveAsync_NullObject_ThrowsArgumentNullException()
        {
            //Arrange
            //Act
            //Assert
            await FluentActions.Invoking(() => _authorRepository.RemoveAsync(null)).Should().ThrowAsync<ArgumentNullException>();
        }

        #endregion

        #region update-async

        [Fact]
        public async Task UpdateAsync_ValidObject_UpdatesSuccessfully()
        {
            //Arrange
            Author author = _authors.First();
            author.Should().NotBeNull();

            Author updatedAuthor = new Author()
            {
                Id = author.Id,
                Name = string.Concat(author.Name, "-modified"),
                Biography = string.Concat(author.Biography, "-modified")
            };

            //Act
            DateTime timeBeforeUpdate = DateTime.UtcNow;
            var result = await _authorRepository.UpdateAsync(author.Id, updatedAuthor);
         
            //Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(updatedAuthor.Id);
            result?.Name.Should().Be(updatedAuthor.Name);
            result?.Biography.Should().Be(updatedAuthor.Biography);

            result?.LastModifiedAt.Should().BeAfter(timeBeforeUpdate);
            result?.LastModifiedAt.Should().BeAfter(updatedAuthor.LastModifiedAt);
        }

        [Fact]
        public async Task UpdateAsync_UpdateOnlySpecificField_UpdatesCorrectly()
        {
            //Arrange
            Author author = _authors.First();
            string originalBiography = author.Biography;
            string updatedName = author.Name + " Updated";

            //Act
            author.Name = updatedName;
            var result = await _authorRepository.UpdateAsync(author.Id, author);

            //Assert
            result.Name.Should().Be(updatedName);
            result.Biography.Should().Be(originalBiography);
        }


        [Fact]
        public async Task UpdateAsync_NoModification_UpdatesSuccessfully()
        {
            //Arrange
            Author author = _authors.First();
            author.Should().NotBeNull();

            Author updatedAuthor = new Author()
            {
                Id = author.Id,
                Name = author.Name
            };

            //Act
            var result = await _authorRepository.UpdateAsync(author.Id, updatedAuthor);
            
            //Assert
            result.Should().NotBeNull();
            result?.Id.Should().Be(author.Id);
            result?.Name.Should().Be(author.Name);
        }

        [Fact]
        public async Task UpdateAsync_IdModification_ThrowsInvalidOperationException()
        {
            //Arrange
            Author author = _authors.First();
            author.Should().NotBeNull();

            Author updatedAuthor = new Author()
            {
                Id = author.Id + 1,
                Name = author.Name
            };

            //Act
            //Assert
            await FluentActions.Invoking(() => _authorRepository.UpdateAsync(author.Id, updatedAuthor))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task UpdateAsync_InvalidObject_ThrowsInvalidOperationException()
        {
            //Act
            var lastItem = _context.Authors.OrderByDescending(x => x.Id).First();
           
            Author author = new Author()
            {
                Id = lastItem.Id + 1,
                Name = "Uknown Author"
            };

            var matchingObject = await _authorRepository.GetAsync(x => x.Name == author.Name);
            matchingObject.Should().BeNull();

            await FluentActions.Invoking(() => _authorRepository.UpdateAsync(author.Id, author))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task UpdateAsync_NullObject_ThrowsArgumentNullException()
        {
            //Arrange
            //Act
            //Assert
            await FluentActions.Invoking(() => _authorRepository.UpdateAsync(1, null)).Should().ThrowAsync<ArgumentNullException>();
        }
                                                                               
        #endregion

        #endregion

        #region utility-methods

        Author GetDanBrown()
        {
            return new Author()
            {
                Id = 10,
                Name = "Dan Brown"
            };
        }

        Author GetAngelaDuckworth()
        {
            return new Author()
            {
                Id = 11,
                Name = "Angela Duckworth"
            };
        }

        #endregion
    }
}
