using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;

namespace LibraryManagementSystem.Tests
{
    public abstract class TestBase
    {
        protected ITestOutputHelper _outputHelper;

        public TestBase(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        protected bool ValidateObject<T>(T validationObject)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(validationObject);
           
            return Validator.TryValidateObject(validationObject, validationContext, validationResults);
        }

        protected void AssertValidationResults(bool result, bool isSuccessExpected)
        {
            if (isSuccessExpected)
                result.Should().BeTrue();
            else
                result.Should().BeFalse();
        }
    }
}