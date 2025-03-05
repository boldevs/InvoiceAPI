using Application.Abstractions.Authentication;
using Domain.Entities.Users;
using Infrastructure.AspUserSecurity.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AspUserSecurity.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckPasswordAsync(Users userDto, string password)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user == null)
                return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<Users?> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            return new Users
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public Task<Users?> GetUserByIdAsync(Guid userId)
        {
            return _userManager.Users
                .Where(u => u.Id == userId)
                .Select(u => new Users
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IList<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return new List<string>();

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<List<Users>> GetUsersByIdsAsync(List<Guid> userIds)
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var users = allUsers
                        .Where(u => userIds.Contains(Guid.Parse(u.Id.ToString())))
                        .ToList();

            return users.Select(user => new Users
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }).ToList();
        }

        public async Task<bool> RegisterUserAsync(string email, string password, string firstName, string lastName, string role)
        {
            var user = new User
            {
                Email = email,
                UserName = email,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return false;

            // ✅ Assign Role to the User
            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

        public async Task<bool> UserExistsByIdAsync(Guid userId)
        {
            return await _userManager.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
