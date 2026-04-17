using GraduationProject.Services.Abstraction;
using GraduationProject.Shared.DTOs.ParentDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ParentController :ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentController(IParentService parentService)
        {
            _parentService = parentService;
        }
        [HttpGet("{specialistId}/parents")]
        public async Task<ActionResult<IEnumerable<ParentGetNameDTO>>> GetParents(int specialistId)
        {
            var parents = await _parentService.GetParentsBySpecialistIdAsync(specialistId);

            if (parents == null || !parents.Any())
                return NotFound("No parents found for this specialist.");

            return Ok(parents);
        }


    }
}
