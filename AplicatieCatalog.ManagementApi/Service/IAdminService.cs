using AplicatieCatalog.ManagementApi.DTOs;

namespace AplicatieCatalog.ManagementApi.Service
{
    public interface IAdminService
    {
        Task addMaterie(AddMaterie dto);
        Task deleteUser(string userId);

        Task<List<UserAdmin>> SearchUsersAsync(string term);

    }
}
