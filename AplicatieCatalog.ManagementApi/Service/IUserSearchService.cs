using AplicatieCatalog.Models;
using AplicatieCatalog.ManagementApi.DTOs;
namespace AplicatieCatalog.ManagementApi.Service
{
    public interface IUserSearchService
    {
        Task<List<UserDTO>> SearchUsersAsync(string query);
       
    }
}
