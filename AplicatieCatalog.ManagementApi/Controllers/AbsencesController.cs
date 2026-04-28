using AplicatieCatalog.ManagementApi.DTOs;
using AplicatieCatalog.ManagementApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AplicatieCatalog.ManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbsencesController : ControllerBase
    {
        private readonly IAbsences _absencesService;

        public AbsencesController(IAbsences absencesService)
        {
            _absencesService = absencesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddAbsences([FromBody] AddAbsenceDTO dto)
        {
            try
            {
                await _absencesService.AddAbsencesAsync(dto);

                return Ok("Absenta a fost adaugata cu succes!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }


        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentAbsences(int studentId)
        {
            try
            {
                var absences = await _absencesService.GetStudentAbsencesAsync(studentId);

                return Ok(absences);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        



    }
}
