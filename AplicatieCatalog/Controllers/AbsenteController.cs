using AplicatieCatalog.Data;
using AplicatieCatalog.Hubs;
using AplicatieCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace AplicatieCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsenteController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AbsenteController(IHttpClientFactory httpClient, ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _httpClient = httpClient.CreateClient();
            _hubContext = hubContext;
        }


        [Authorize(Roles = "Teacher")]
        [HttpGet("add-absenta-data")]
        public async Task<IActionResult> getAddAbsentaData()
        {

            var students = await _context.Students
                .Select(s => new
                {
                    id = s.ID,
                    fullname = s.LastName + " " + s.FirstName
                })
                .ToListAsync();

            var materii = await _context.Materii
                .Select(m => new
                {
                    id = m.Id,
                    name = m.Nume
                })
                .ToListAsync();

            return Ok(new
            {
                students,
                materii
            });
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("add-absenta")]
        public async Task<IActionResult> AddAbsenta([FromBody] AbsentaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var profesor = await _context.Profesori
                .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

            if (profesor == null)
            {
                return NotFound(new { message = "Profesorul nu a fost gasit!" });
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ID == model.StudentId);

            if (student == null)
            {
                return NotFound(new { message = "Studentul nu a fost gasit!" });
            }

            var studentUserId = student.ApplicationUserId;

            var request = new AbsenteRequest
            {
                StudentId = model.StudentId,
                ProfesorId = profesor.ID,
                MaterieId = model.MaterieId

            };

            var response = await _httpClient.PostAsJsonAsync(

                   "https://localhost:7197/api/Absences",
                    request
                );

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BadRequest(new { message = errorMessage });
            }

            await _hubContext.Clients.Group(studentUserId).SendAsync("ReceiveNotification", "A fost adaugata o absenta noua.");


            return Ok(new { message = "Absenta adaugata cu succes!" });
        }


        [Authorize(Roles = "Student")]
        [HttpGet("my-absences")]
        public async Task<IActionResult> MyGrades()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

            if (student == null)
            {
                return NotFound(new { errorMessage = "Studentul nu a fost gasit!" });
            }


            var apiUrl = $"https://localhost:7197/api/Absences/student/{student.ID}";

            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new {message = "Eroare la request!"});
            }

            var json = await response.Content.ReadAsStringAsync();

            var absences = JsonSerializer.Deserialize<List<AbsentaViewModel>>(

                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }

                );

            return Ok(absences ?? new List <AbsentaViewModel>());
        }

    }
}
