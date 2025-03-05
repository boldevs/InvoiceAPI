using Application.Features.Customers.Create;
using FluentValidation.TestHelper;

namespace UnitTests.Application.Features.Customers
{
    public class CreateCustomerCommandValidatorTests
    {
        private readonly CreateCustomerCommandValidator _validator;

        public CreateCustomerCommandValidatorTests()
        {
            _validator = new CreateCustomerCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var command = new CreateCustomerCommand { Name = "" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var command = new CreateCustomerCommand { Email = "invalid-email" };
            var result = _validator.TestValidate(command);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Phone_Is_Not_E164_Format()
        {
            var invalidPhones = new[]
            {
            "12345",          // Too short
            "+00123456789",   // Starts with 0 (invalid E.164)
            "abcd123456",     // Contains letters
            "+1234567890123456", // Too long (E.164 max is 15 digits)
            "++1234567890"    // Double `+` sign
        };

            foreach (var phone in invalidPhones)
            {
                var command = new CreateCustomerCommand
                {
                    Name = "Valid Name",
                    Email = "valid@example.com",
                    Phone = phone,
                    Address = "123 Main St"
                };

                var result = _validator.TestValidate(command);
                result.ShouldHaveValidationErrorFor(c => c.Phone);
            }
        }

        [Fact]
        public void Should_Pass_When_Valid_Command_Is_Provided()
        {
            var command = new CreateCustomerCommand
            {
                Name = "John Doe",
                Email = "john@gmail.com",
                Phone = "+1234567890",
                Address = "123 Main"
            };

            var result = _validator.TestValidate(command);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
