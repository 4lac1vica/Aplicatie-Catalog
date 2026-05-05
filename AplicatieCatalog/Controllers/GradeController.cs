using AplicatieCatalog.Data;
using AplicatieCatalog.Models;
using AplicatieCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using AplicatieCatalog.Hubs;

namespace AplicatieCatalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradesController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext; 

        public GradesController(IHttpClientFactory httpClientFactory, ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("add-data")]
        public async Task<IActionResult> GetAddGradeData()
        {
            var students = await _context.Students
                .Select(s => new
                {
                    id = s.ID,
                    fullName = s.FirstName + " " + s.LastName
                })
                .ToListAsync();


            var materii = await _context.Materii
                .Select(m => new
                {
                    id = m.Id,
                    nume = m.Nume
                })
                .ToListAsync();


            return Ok(new
            {
                students,
                materii
            });
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("add")]
        public async Task<IActionResult> AddGrade([FromBody] AddGradesViewModel model)
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

            var request = new AddGradeRequest
            {
                StudentId = model.StudentId,
                ProfesorId = profesor.ID,
                MaterieId = model.MaterieId,
                Valoare = model.Valoare

            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://localhost:7197/api/AddGrades",
                request
            );

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return BadRequest(new { message = errorMessage });
            }


            var student = await _context.Students.FirstOrDefaultAsync(s => s.ID == model.StudentId);

            var materie = await _context.Materii
                .FirstOrDefaultAsync(m => m.Id == model.MaterieId);

            var notification = new Notification
            {
                UserId = student.ApplicationUserId,
                Message = $"Ai primit o noua nota la {materie.Nume}",
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(student.ApplicationUserId)
                .SendAsync("ReceiveNotification", notification.Message);



            return Ok(new { message = "Nota a fost adaugata cu succes!" });


        }

        [Authorize(Roles = "Student")]
        [HttpGet("my-grades")]
        public async Task<IActionResult> MyGrades()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

            if (student == null)
            {
                return NotFound(new { message = "Studentul nu a fost gasit!" });
            }

            var apiUrl = $"https://localhost:7197/api/AddGrades/student/{student.ID}";

            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new { message = "Nu se pot incarca notele." });
            }

            var json = await response.Content.ReadAsStringAsync();

            var grades = JsonSerializer.Deserialize<List<StudentGradeViewModel>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }

                );

            return Ok(grades ?? new List<StudentGradeViewModel>());
        }

       
        



    }
}
