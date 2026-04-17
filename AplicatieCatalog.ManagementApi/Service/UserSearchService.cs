using AplicatieCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AplicatieCatalog.ManagementApi.DTOs;

namespace AplicatieCatalog.ManagementApi.Service
{
    public class UserSearchService : IUserSearchService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSearchService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserDTO>> SearchUsersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<UserDTO>();

            query = query.ToLower();

            var users = await _userManager.Users.
                Where(u =>
                (u.Email != null && u.Email.ToLower().Contains(query)) ||
                (u.FirstName != null && u.FirstName.ToLower().Contains(query)) ||
                (u.LastName != null && u.LastName.ToLower().Contains(query))).ToListAsync();

            var result = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var visibleRole = roles.FirstOrDefault(r => r != "Admin") ?? "No role";

                result.Add(new UserDTO
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = visibleRole
                });
            }

            return result;

        }


    }
}
