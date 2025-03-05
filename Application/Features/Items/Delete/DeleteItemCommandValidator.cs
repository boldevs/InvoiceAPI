using FluentValidation;

namespace Application.Features.Items.Delete
{
    internal sealed class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
    {
        public DeleteItemCommandValidator()
        {
            RuleFor(c => c.itemId).NotEmpty();
        }
    }
}
