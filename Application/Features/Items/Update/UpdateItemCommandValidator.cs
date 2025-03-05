using Application.Features.Items.Update;
using FluentValidation;

namespace Application.Features.Items.Create
{
    internal sealed class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemCommandValidator()
        {
            RuleFor(c => c.Name)
               .NotEmpty()
               .MaximumLength(100);

            RuleFor(c => c.BarCode)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.Descriptions)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Description cannot exceed 255 characters.");

            RuleFor(c => c.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Unit price must be a positive value.");
        }
    }
}
