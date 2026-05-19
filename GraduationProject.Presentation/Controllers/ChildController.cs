using GraduationProject.Services;
using GraduationProject.Services.Abstraction;
using GraduationProject.Services.Exceptions;
using GraduationProject.Shared.DTOs.ChildDTOs;
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
    public class ChildController :ControllerBase
    {
        private readonly IChildServices _childServices;

        public ChildController(IChildServices childServices)
        {
            _childServices = childServices;
        }

        [HttpGet("track/{childId}")]
        public async Task<IActionResult> TrackChildProgress(int childId)
        {
            try
            {
                var result = await _childServices.GetChildProgressAsync(childId);
                return Ok(result);
            }
            catch (ChildNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("SpecialistId/{SpecialistId}")]
        public async Task<ActionResult<IEnumerable<ChildDTO>>> GetChildrenBySpecialist(int SpecialistId)
        {
            var children = await _childServices.GetChildrenBySpecialistIdAsync(SpecialistId);

            if (children == null || !children.Any())
                return NotFound("No children found for this Specialist.");

            return Ok(children);
        }

        [HttpGet("parent/{parentId}")]
        public async Task<ActionResult<IEnumerable<ChildNameDTO>>> GetChildrenByParent(int parentId)
        {
            var children = await _childServices.GetChildrenByParentIdAsync(parentId);

            if (children == null || !children.Any())
                return NotFound("No children found for this parent.");

            return Ok(children);
        }

    }
}
