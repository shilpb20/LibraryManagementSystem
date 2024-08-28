using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;

namespace LibraryManagementSystem.Tests
{
    public class TestBase
    {
        protected ITestOutputHelper _outputHelper;

        public TestBase(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        protected List<ValidationResult> ValidateAndAssert<T>(T validationObject, bool isSuccessExpected, string errorMessage = "")
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool result = Validate<T>(validationObject, validationResults, errorMessage);
            if (isSuccessExpected)
            {
                result.Should().BeTrue();
            }
            else
            {
                result.Should().BeFalse();
            }

            return validationResults;
        }

        protected bool Validate<T>(T validationObject, List<ValidationResult> validationResults, string errorMessage = null)
        {
            ValidationContext validationContext = new ValidationContext(validationObject);
            bool result = Validator.TryValidateObject(validationObject, validationContext, validationResults);
           
            if(validationResults.Any())
            {
                _outputHelper.WriteLine("Error Messages:");
                _outputHelper.WriteLine(string.Join("\n", validationResults));
            }
            
            if(!string.IsNullOrEmpty(errorMessage))
            {
                validationResults.Should().ContainSingle()
                    .Which.ErrorMessage.Should().Be(errorMessage);
            }

            return result;
        }
    }
}