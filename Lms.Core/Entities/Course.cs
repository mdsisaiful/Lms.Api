using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
    public class Course
    {
        public int Id { get; set; } // primary key
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        // Navigation properties
        public ICollection<Module> Modules { get; set; }
        /*
         * The Modules property is defined as ICollection<Module> because there may be 
         * multiple related Module entities.
         */
    }
}
