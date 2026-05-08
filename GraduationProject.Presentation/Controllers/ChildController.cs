using GraduationProject.Services.Abstraction;
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

                if (result == null)
                    return NotFound("This child is not Found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ArgumentException:
                        return BadRequest(ex.Message);
                    case InvalidOperationException:
                        return BadRequest("Invalid Operations ");
                    case OutOfMemoryException:
                        return StatusCode(503, "service is Not Available,Try Again Later");
                    default:
                        return StatusCode(500, "Internal Server Error");
                         
                }




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
