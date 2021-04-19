using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
    public class Course
    {
        [Key]
        public int Id { get; set; } // primary key

        [Required]
        [MaxLength(70)]
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
