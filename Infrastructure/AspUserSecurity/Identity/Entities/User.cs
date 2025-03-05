using Microsoft.AspNetCore.Identity;

namespace Infrastructure.AspUserSecurity.Identity.Entities
{
    public sealed class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
