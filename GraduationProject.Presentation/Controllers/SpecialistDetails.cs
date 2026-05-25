using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.SpecialistDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialistDetails :ControllerBase
    {
        private readonly ISpecialistServices _specialistServices;

        public SpecialistDetails(ISpecialistServices specialistServices)
        {
           _specialistServices = specialistServices;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecialistById(int id)
        {
            var specialist = await _specialistServices.GetDetailsAsync(id);
            if (specialist == null)
                return NotFound("Specialist is Not Found ");

            return Ok(specialist);
        }

        // PUT: api/specialist/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecialist(int id, [FromBody] UpdateSpecialistDetailsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _specialistServices.UpdateAsync(id, dto);
            if (!updated)
                return NotFound(new { Message = "Specialist not found" });

            return NoContent(); // 204 → Success, no response body
        }
    }
}
