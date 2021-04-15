using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public static class SeedData
    {
        //private static Faker fake = new Faker("sv");

        //public SeedData(Faker fake)
        //{
        //    this.fake = new Faker("sv");
        //}
        public static async Task InitAsync(IServiceProvider services)
        {
            using (var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                //if (await context.Courses.AnyAsync()) return;


                var fake = new Faker("sv");


                var courses = GetCourses();
                var modules = GetModules();

                //await context.AddRangeAsync(modules);

                // Skriv kod för att seeda några kurser som alla har några tillhörande moduler.

                foreach (var course in courses)
                {
                    var someModules = new List<Module>();
                    var r = new Random();
                    //var r = fake.Random.Int(1, 20);
                    //var someModules = fake.Random(0, 3);

                    for (int i = 0; i < 5; i++)
                    {
                        someModules.Add(modules[r.Next(1,20)]);
                    }


                    course.Modules = someModules;
                    
                }

                await context.AddRangeAsync(courses);
                //context.AddRange(courses);
                await context.SaveChangesAsync();
            }
        }

        private static List<Course> GetCourses()
        {
            var fake = new Faker("sv");
            var courses = new List<Course>();

            for (int i = 0; i < 20; i++)
            {
                var course = new Course
                {
                    Title = fake.Company.CatchPhrase(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-2, 2))
                };

                courses.Add(course);
            }

            return courses;
        }

        private static List<Module> GetModules()
        {
            var fake = new Faker("sv");
            var modules = new List<Module>();


            for (int i = 0; i < 20; i++)
            {
                var module = new Module
                {
                    Title = fake.Company.CatchPhrase(),
                    StartDate = DateTime.Now.AddDays(fake.Random.Int(-2, 2))
                };

                modules.Add(module);
            }
            return modules;
        }
    }
}
