using Application.Abstractions.Messaging;

namespace Application.Features.Users.Register
{
    public sealed class RegisterUserCommand : ICommand<Guid>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
    }
}
