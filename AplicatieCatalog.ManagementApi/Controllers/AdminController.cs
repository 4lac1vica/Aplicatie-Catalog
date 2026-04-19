using AplicatieCatalog.ManagementApi.DTOs;
using AplicatieCatalog.ManagementApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AplicatieCatalog.ManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
       private readonly IAdminService _adminService;
        
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("add-materie")]
        public async Task<IActionResult> AddMaterie([FromBody] AddMaterie dto)
        {
            try
            {
                await _adminService.addMaterie(dto);
                return Ok("Materia a fost adaugata!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search-users")]
        public async Task<IActionResult> SearchUsers(string term)
        {
            var users = await _adminService.SearchUsersAsync(term);
            return Ok(users);
        }



        [HttpPost("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                await _adminService.deleteUser(userId);
                return Ok("User sters");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
