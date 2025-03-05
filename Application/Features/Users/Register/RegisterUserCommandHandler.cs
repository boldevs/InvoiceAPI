using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Features.Users.Register
{
    internal sealed class RegisterUserCommandHandler(IUserService userService)
        : ICommandHandler<RegisterUserCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var success = await userService.RegisterUserAsync(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                "Admin"
            );

            return Result.Success(Guid.NewGuid());
        }
    }
}
