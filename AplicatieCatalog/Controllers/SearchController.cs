using Microsoft.AspNetCore.Mvc;
using AplicatieCatalog.ViewModels;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using AplicatieCatalog.Data;


namespace AplicatieCatalog.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly HttpClient _httpClient;
       
        
        public SearchController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("users")]
        public async Task<IActionResult> Users([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new List<UserSearchViewModel>());

            var apiUrl = $"https://localhost:7197/api/user-search/users?query={Uri.EscapeDataString(query)}"; ;

            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new { message = "Nu se pot incarca utilizatorii!" });
            }

            var json = await response.Content.ReadAsStringAsync();

            var users = JsonSerializer.Deserialize<List<UserSearchViewModel>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }

            );

            return Ok(users ?? new List<UserSearchViewModel>());

        }


    }
}
