using FluentAssertions;
using LibraryManagementSystem.Core.Models;
using Xunit.Abstractions;

namespace LibraryManagementSystem.Tests.Models
{
    public class AuthorTests : TestBase
    {
        private readonly int _id = 1;
        private readonly string _authorName = "Author 1";
        private readonly string _biography = "Sample author biography..";

        private const string errorMessage_NameField_Required = "The Name field is required.";    

        public AuthorTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {

        }

        [Fact]
        public void CreateAuthor_ValidObject_InitializeAllProperties_CreatesSuccessfully()
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
        public void CreateAuthor_ValidObject_InitializeOnlyRequiredProperties_CreatesSuccessfully()
        {
            //Arrange
            //Act
            Author author = CreateAuthor(x => x.Biography = "");

            //Assert
            author.Should().NotBeNull();
            author.Id.Should().Be(_id);
            author.Name.Should().Be(_authorName);
            author.Biography.Should().BeNull();
        }

        [Fact]
        public void CreateAuthor_Id_NoInitialization_PassesValidation()
        {
            //Arrange
            //Act
            Author author = new Author()
            {
                Name = _authorName,
                Biography = _biography
            };

            //Assert
            author.Should().NotBeNull();
            ValidateAndAssert(author, true);
        }

        [Fact]
        public void CreateAuthor_Name_NoInitialization_FailsValidation()
        {
            //Arrange
            //Act
            Author author = new Author()
            {
                Id = _id,
                Biography = _biography
            };

            //Assert
            author.Should().NotBeNull();
            ValidateAndAssert(author, false);        }


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
                Name = name, 
                Biography = _biography
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
                Name = _authorName,
                Biography = _biography
            };

            setup?.Invoke(author);

            return author;
        }
    }
}
