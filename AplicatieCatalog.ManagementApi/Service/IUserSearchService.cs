using AplicatieCatalog.Models;
namespace AplicatieCatalog.ManagementApi.Service
{
    public interface IUserSearchService
    {
        Task<List<ApplicationUser>> SearchUsersAsync(string query);
       
    }
}
