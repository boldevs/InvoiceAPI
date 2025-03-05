using Domain.Entities.Users;

namespace Application.Abstractions.Authentication
{
    public interface IUserService
    {
        Task<Users?> GetUserByEmailAsync(string email);
        Task<List<Users>> GetUsersByIdsAsync(List<Guid> userIds);
        Task<Users?> GetUserByIdAsync(Guid userId);
        Task<bool> RegisterUserAsync(string email, string password, string firstName, string lastName, string role);
        Task<bool> CheckPasswordAsync(Users user, string password);
        Task<IList<string>> GetUserRolesAsync(Guid userId);
        Task<bool> UserExistsByIdAsync(Guid userId);
    }
}
