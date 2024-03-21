using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class Profile
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string StudentId { get; set; }
        public string Email { get; set; }
    }
}
