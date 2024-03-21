using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class CourseEnrollment
    {
        [Key]
        public int CourseEnrollmentId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }

    }
}