using AplicatieCatalog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;



namespace AplicatieCatalog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        
        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> SearchUsers(string term)
        {
            List<ApplicationUserViewModel> users = new();

            if (!string.IsNullOrWhiteSpace(term))
            {
                users = await _httpClient.GetFromJsonAsync<List<ApplicationUserViewModel>>(
                    $"https://localhost:7197/api/Admin/search-users?term={term}"
                    ) ?? new List<ApplicationUserViewModel>();
            }

            ViewBag.Term = term;
            return View(users);
        }



        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await _httpClient.DeleteAsync($"https://localhost:7197/api/Admin/delete-user/{userId}");

            return RedirectToAction("SearchUsers");
        }

        [HttpGet]
        public IActionResult AddMaterie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMaterie(AddMaterieViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7197/api/Admin/add-materie",
                model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Materia a fost adaugata!";
                return RedirectToAction("AddMaterie"); 
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", error);

            return View(model);

        }

        

        


    }
}
