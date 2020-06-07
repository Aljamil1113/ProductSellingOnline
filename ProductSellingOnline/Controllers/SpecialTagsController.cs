using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSellingOnline.Dtos;
using ProductSellingOnline.Services;

namespace ProductSellingOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialTagsController : ControllerBase
    {
        private ISpecialTagServices SpecialTagServices { get; set; }

        public SpecialTagsController(ISpecialTagServices _SpecialTagServices)
        {
            SpecialTagServices = _SpecialTagServices;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SpecialTagDto>))]
        public IActionResult GetSpecialTags()
        {
            var specialTags = SpecialTagServices.GetSpecialTags();

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var specialTagDtos = new List<SpecialTagDto>();

            foreach (var specialTag in specialTags)
            {
                specialTagDtos.Add(new SpecialTagDto
                {
                    Id = specialTag.Id,
                    Name = specialTag.Name
                });
            }

            return Ok(specialTagDtos);
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(SpecialTagDto))]
        public IActionResult GetSpecialTag(int id)
        {
            if(!SpecialTagServices.IsSpecialTagExist(id))
            {
                return NotFound();
            }

            var specialTag = SpecialTagServices.GetSpecialTag(id);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var specialTagDto = new SpecialTagDto
            {
                Id = specialTag.Id,
                Name = specialTag.Name
            };

            return Ok(specialTagDto);
        }
    }
}
