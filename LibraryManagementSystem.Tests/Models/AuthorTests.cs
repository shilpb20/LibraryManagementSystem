using FluentAssertions;
using LibraryManagementSystem.Core.Models;
using Xunit.Abstractions;

namespace LibraryManagementSystem.Tests.Models
{
    public class AuthorTests : TestBase
    {
        private readonly int _id = 1;
        private readonly string _authorName = "Author 1";

        private const string errorMessage_NameField_Required = "The Name field is required.";    

        public AuthorTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {

        }

        [Fact]
        public void CreateAuthor_ValidObject_CreatesSuccessfully()
        {
            //Arrange
            //Act
            Author author = CreateAuthor();

            //Assert
            author.Should().NotBeNull();
            author.Id.Should().Be(_id);
            author.Name.Should().Be(_authorName);
        }

        [Fact]
        public void CreateAuthor_MissingIdInitialization_PassesValidation()
        {
            //Arrange
            //Act
            Author author = new Author()
            {
                Name = _authorName,
            };

            //Assert
            author.Should().NotBeNull();
            var validationResult = ValidateAndAssert(author, true);
        }

        [Fact]
        public void CreateAuthor_MissingNamenitialization_FailsValidation()
        {
            //Arrange
            //Act
            Author author = new Author()
            {
                Id = _id,
            };

            //Assert
            author.Should().NotBeNull();
            var validationResult = ValidateAndAssert(author, false);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public void CreateAuthor_Length_Valid_PassesValidation(int length)
        {
            //Arrange
            //Act
            string name = new string('a', length);
            var author = CreateAuthor(a => a.Name = name);

            //Assert
            author.Should().NotBeNull();
            ValidateAndAssert(author, true);
        }

        [Theory]
        [InlineData(0, errorMessage_NameField_Required)]
        public void CreateAuthor_Length_Invalid_FailsValidation(int length, string errorMessage)
        {
            //Arrange
            //Act
            string name = new string('a', length);
            var author = new Author()
            {
                Id = _id,
                Name = name
            };

            _outputHelper.WriteLine($"String length - {author.Name.Length}");

            //Assert
            author.Should().NotBeNull();
            ValidateAndAssert(author, false);
        }

        private Author CreateAuthor(Action<Author>? setup = null)
        {
            var author = new Author()
            {
                Id = _id,
                Name = _authorName
            };

            setup?.Invoke(author);

            return author;
        }
    }
}
