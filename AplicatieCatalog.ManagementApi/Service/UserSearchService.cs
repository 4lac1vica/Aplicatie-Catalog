using AplicatieCatalog.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AplicatieCatalog.ManagementApi.Service
{
    public class UserSearchService : IUserSearchService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public UserSearchService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> SearchUsersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<ApplicationUser>();

            query = query.ToLower();

            return await _userManager.Users
                .Where(u =>
                (u.Email != null && u.Email.ToLower().Contains(query)) ||
                (u.FirstName != null && u.FirstName.ToLower().Contains(query)) ||
                (u.LastName != null && u.LastName.ToLower().Contains(query))).
                ToListAsync();
        }

    }
}
