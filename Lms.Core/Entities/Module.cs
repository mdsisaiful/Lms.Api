using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
    public class Module
    {
        [Key]
        public int Id { get; set; }     // primary key

        [Required]
        [MaxLength(70)]
        public string Title { get; set; }
        public DateTime StartDate { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }   // foreign key

        /*
         * CourseId is the foreign key for the Course navigation property
         */

        // Navigation Property
        public Course Course { get; set; }
    }
}
