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
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext db;
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public CoursesController(ApplicationDbContext context, IUoW uow, IMapper mapper)
        {
            db = context;
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet()]
        [HttpHead]

        //public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        //{
        //    var coursesFromRepo = await uow.CourseRepository.GetAllCourses();
        //    var coursesDto = mapper.Map<IEnumerable<CourseDto>>(coursesFromRepo);
        //    return Ok(coursesDto);
        //}

        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(bool includeModels = false)
        {
            //var coursesFromRepo = await uow.CourseRepository.GetAllCourses();
            var coursesFromRepo = await uow.CourseRepository.GetAllCourses(includeModels);
            var coursesDto = mapper.Map<IEnumerable<CourseDto>>(coursesFromRepo);
            return Ok(coursesDto);
        }

        // GET: api/Courses/5
        [HttpGet("{id}", Name = "GetCourse")]
        public async Task<ActionResult<CourseDto>> GetCourse(int? id)
        {
            var courseFromRepo = await uow.CourseRepository.GetCourse(id);
            if (courseFromRepo == null)
            {
                return BadRequest();
            }

            var courseDto = mapper.Map<CourseDto>(courseFromRepo);
            return Ok(courseDto);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<CourseDto>> PutCourse(int id, CourseDto courseDto)
        {
            var courseFromRepo = await uow.CourseRepository.GetCourse(id);

            if (courseFromRepo is null)
            {
                return NotFound();
            }

            mapper.Map(courseDto, courseFromRepo);
            if (await uow.ModuleRepository.SaveAsync())
            {
                return Ok(mapper.Map<CourseDto>(courseFromRepo));
            }
            else
            {
                return StatusCode(500);
            }
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseDto courseDto)
        {
            var courseEntity = mapper.Map<Course>(courseDto);
            await uow.CourseRepository.AddAsync(courseEntity);
            await uow.CourseRepository.SaveAsync();

            var courseToReturn = mapper.Map<CourseDto>(courseEntity);
            //return CreatedAtRoute("GetCourse", 
            //    new { id = courseToReturn.Title }, 
            //    courseToReturn);
            //return Ok(courseToReturn);
            return Ok();
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var courseFromRepo = await uow.CourseRepository.GetCourse(id);
            if (courseFromRepo == null)
            {
                return NotFound();
            }

            uow.CourseRepository.RemoveCourse(courseFromRepo);
            await uow.CourseRepository.SaveAsync();

            return NoContent();
        }

        

        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId,
            JsonPatchDocument<CourseDto> patchDocument)
        {
            var course = await uow.CourseRepository.GetCourse(courseId);

            if (course is null)
            {
                return NotFound();
            }

            var model = mapper.Map<CourseDto>(course);
            
            patchDocument.ApplyTo(model, ModelState);

            if (!TryValidateModel(model))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(model, course);

            if (await uow.CourseRepository.SaveAsync())
            {
                return Ok(mapper.Map<CourseDto>(model));
            }
            else
            {
                return StatusCode(500);
            }

        }

        private bool CourseExists(int id)
        {
            return uow.CourseRepository.Any(id);
        }
    }
}
