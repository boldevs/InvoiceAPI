using Application.Abstractions.Messaging;

namespace Application.Features.Users.Login
{
    public sealed class LoginUserCommand : ICommand<LoginResult>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
