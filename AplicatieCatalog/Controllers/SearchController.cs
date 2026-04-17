using Microsoft.AspNetCore.Mvc;
using AplicatieCatalog.ViewModels;
using System.Text.Json;


namespace AplicatieCatalog.Controllers
{
    public class SearchController : Controller
    {
        private readonly HttpClient _httpClient;
        
        public SearchController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Users (string query)
        {
            var model = new UserSearchPageViewModel
            {
                Query = query
            };

            if (string.IsNullOrWhiteSpace(query))
                return View(model);

            var apiUrl = $"https://localhost:7197/api/Search/users?query={Uri.EscapeDataString(query)}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                var users = JsonSerializer.Deserialize<List<UserSearchViewModel>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                model.Results = users ?? new List<UserSearchViewModel>();
            }

            return View(model);
        }
        
    }
}
