using AplicatieCatalog.ManagementApi.DTOs;
using AplicatieCatalog.ManagementApi.Service;
using Microsoft.AspNetCore.Mvc;



namespace AplicatieCatalog.ManagementApi.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
        public class SearchController : ControllerBase
        {
            private readonly IUserSearchService _userSearchService;
            
            public SearchController(IUserSearchService userSearchService)
            {
                _userSearchService = userSearchService;
            }

            [HttpGet("users")]
            public async Task<IActionResult> SearchUsers(string query)
            {
                if (string.IsNullOrWhiteSpace(query))
                    return BadRequest("Eroare la query!");

                var users = await _userSearchService.SearchUsersAsync(query);


                return Ok(users);
            }
           
        }

   }

