using FluentValidation;

namespace Application.Features.Customers.Delete
{
    internal sealed class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(c => c.customerId).NotEmpty();
        }
    }
}
