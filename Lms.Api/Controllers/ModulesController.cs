using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Core.Repositories;
using AutoMapper;
using Lms.Core.Dto;

namespace Lms.Api.Controllers
{
    [Route("api/courses/{courseId}/modules")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public ModulesController(ApplicationDbContext context, IUoW uow, IMapper mapper)
        {
            _context = context;
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModules()
        {
            var modulesFromRepo = await uow.ModuleRepository.GetAllModules();
            var modulesDto = mapper.Map<IEnumerable<ModuleDto>>(modulesFromRepo);
            return Ok(modulesDto);
        }

        // GET: api/Modules/5
        [HttpGet("{id}", Name = "GetModuleForCourse")]
        public async Task<ActionResult<ModuleDto>> GetModule(int? id)
        {
            var moduleFromRepo = await uow.ModuleRepository.GetModule(id);
            var moduleDto = mapper.Map<ModuleDto>(moduleFromRepo);
            if (moduleFromRepo == null)
            {
                return BadRequest();
            }
            return Ok(moduleDto);
        }

        //// GET: api/Modules/5
        //[HttpGet("{title}")]
        //public async Task<ActionResult<ModuleDto>> GetModule(string title)
        //{
        //    var moduleFromRepo = await uow.ModuleRepository.GetModule(title);
        //    var moduleDto = mapper.Map<ModuleDto>(moduleFromRepo);
        //    if (moduleFromRepo == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(moduleDto);
        //}

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ModuleDto>> PutModule(int id, ModuleDto moduleDto)
        {
            var moduleFromRepo = await uow.ModuleRepository.GetModule(id);

            if (moduleFromRepo is null)
            {
                return NotFound();
            }

            mapper.Map(moduleDto, moduleFromRepo);
            if (await uow.ModuleRepository.SaveAsync())
            {
                return Ok(mapper.Map<ModuleDto>(moduleFromRepo));
            }
            else
            {
                return StatusCode(500);
            }
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ModuleDto>> PostModule(ModuleDto moduleDto)
        {
            var moduleEntity = mapper.Map<Module>(moduleDto);
            await uow.CourseRepository.AddAsync(moduleEntity);
            await uow.CourseRepository.SaveAsync();

            var moduleToReturn = mapper.Map<ModuleDto>(moduleEntity);
            //return CreatedAtRoute("GetModuleForCourse", new { title = moduleToReturn.Title }, moduleToReturn);
            return Ok();
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var moduleFromRepo = await uow.ModuleRepository.GetModule(id);
            if (moduleFromRepo == null)
            {
                return NotFound();
            }

            uow.ModuleRepository.RemoveModule(moduleFromRepo);
            await uow.ModuleRepository.SaveAsync();

            return NoContent();
        }

        private bool ModuleExists(int id)
        {
            return uow.ModuleRepository.Any(id);
        }
    }
}
