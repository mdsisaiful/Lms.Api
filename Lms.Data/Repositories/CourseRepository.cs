using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext db;

        public CourseRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddAsync<T>(T added)
        {
            await db.AddAsync(added);
        }

        public bool Any(int id)
        {
            return db.Courses.Any(c => c.Id == id);
        }

        //public async Task<IEnumerable<Course>> GetAllCourses()
        //{
        //    return await db.Courses.ToListAsync();

        //}

        public async Task<IEnumerable<Course>> GetAllCourses(bool includeModules)
        {
            if (includeModules)
            {
                return await db.Courses.Include(m => m.Modules).ToListAsync();
            }
            else
            {
                return await db.Courses.ToListAsync();
            }
        }


        public Task<Course> GetCourse(int? Id)
        {
            return db.Courses.FirstOrDefaultAsync(c => c.Id == Id);
        }

        public void RemoveCourse(Course course)
        {
            db.Remove(course);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }

    }
}
