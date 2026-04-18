using AplicatieCatalog.Data;
using AplicatieCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;


namespace AplicatieCatalog.Controllers
{
    [Authorize]
    public class GradesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;

        public GradesController(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AddGradesViewModel
            {
                Students = await _context.Students
                .Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.FirstName + " " + s.LastName
                })
                .ToListAsync(),


                Materii = await _context.Materii
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Nume
                })
                .ToListAsync()
            };

            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> Add(AddGradesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Students = await _context.Students
                    .Select(s => new SelectListItem
                    {
                        Value = s.ID.ToString(),
                        Text = s.FirstName + " " + s.LastName
                    })
                    .ToListAsync();

                model.Materii = await _context.Materii
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nume
                    })
                    .ToListAsync();
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profesor = await _context.Profesori
                .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);


            if (profesor == null)
            {
                ModelState.AddModelError(string.Empty, "Profesorul nu a fost gasit!");

                model.Students = await _context.Students
                    .Select(s => new SelectListItem
                    {
                        Value = s.ID.ToString(),
                        Text = s.FirstName + " " + s.LastName
                    })
                    .ToListAsync();

                model.Materii = await _context.Materii
                    .Select(m => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Nume
                    })
                    .ToListAsync();

                return View(model);

            }

            var request = new AddGradeRequest
            {
                StudentId = model.StudentId,
                ProfesorId = profesor.ID,
                MaterieId = model.MaterieId,
                Valoare = model.Valoare
            };


            var response = await _httpClient.PostAsJsonAsync(
                    "https://localhost:7197/api/Grades",
                    request
                );

            if (response.IsSuccessStatusCode)
            {
                TempData["Succes"] = "Nota a fost adaugata cu succes!";
                return RedirectToAction(nameof(Add));
            }

            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorMessage);

            model.Students = await _context.Students
                .Select(s => new SelectListItem
                {
                    Value = s.ID.ToString(),
                    Text = s.FirstName + " " + s.LastName
                })
                .ToListAsync();

            model.Materii = await _context.Materii
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Nume
                })
                .ToListAsync();


            return View(model);

        }

        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> MyGrades()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ApplicationUserId == userId);

            if (student == null)
            {
                return NotFound("Studentul nu a putut sa fie gasit");
            }

            var apiUrl = $"https://localhost:7197/api/Grades/student/{student.ID}";

            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Nu se pot incarca notele!!";
                return View(new List<StudentGradeViewModel>());
            }

            var json = await response.Content.ReadAsStringAsync();

            var grades = JsonSerializer.Deserialize<List<StudentGradeViewModel>>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return View(grades ?? new List<StudentGradeViewModel>());



        }
        



    }
}
