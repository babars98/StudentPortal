using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Fee { get; set; }
        public List<CourseEnrollment> CourseEnrollment { get; set; }
    }
}