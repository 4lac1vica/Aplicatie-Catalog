using AplicatieCatalog.ManagementApi.DTOs;
using AplicatieCatalog.ManagementApi.Service;
using Microsoft.AspNetCore.Mvc;



namespace AplicatieCatalog.ManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddGradesController : ControllerBase
    {
        private readonly IAddGrades _gradeService;

        public AddGradesController(IAddGrades gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddNota([FromBody] AddGradeDTO dto)
        {
            try
            {
                await _gradeService.AddNotaAsync(dto);
                return Ok("Nota a fost adaugata cu succes.");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentGrades(int studentId)
        {
           try
            {
                var grades = await _gradeService.GetStudentGradesAsync(studentId);
                return Ok(grades);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
