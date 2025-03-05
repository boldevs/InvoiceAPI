using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using SharedKernel;


namespace Application.Features.Users.Login
{
    public record LoginResult(string Token, Guid UserId);

    internal sealed class LoginUserCommandHandler(IUserService userService, ITokenProvider tokenProvider)
    : ICommandHandler<LoginUserCommand, LoginResult> // 🔄 Change return type
    {
        public async Task<Result<LoginResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserByEmailAsync(request.Email);
            if (user == null || !await userService.CheckPasswordAsync(user, request.Password))
                return Result.Failure<LoginResult>(UserError.InvalidEmail);

            var roles = await userService.GetUserRolesAsync(user.Id);

            var token = tokenProvider.CreateToken(user.Id, user.Email, user.FirstName, user.LastName, roles);

            // ✅ Return both token and user ID
            return Result.Success(new LoginResult(token, user.Id));
        }
    }

}
