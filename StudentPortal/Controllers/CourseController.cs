using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.BL;
using StudentPortal.Data;
using StudentPortal.Models;
using System.Security.Claims;

namespace StudentPortal.Controllers
{
    public class CourseController : Controller
    {

        private ApplicationDbContext _dbContext;

        /// <summary>
        /// Overloaded constructor for Dependency Injection
        /// </summary>
        /// <param name="dbContext"></param>
        public CourseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// List all the courses from Database
        /// </summary>
        /// <returns></returns>
        public IActionResult Courses()
        {
            var courses = _dbContext.Course.ToList();
            return View(courses);
        }

        /// <summary>
        /// View the detail of a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IActionResult CourseDetail(int courseId)
        {
            var course = _dbContext.Course.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isEnrolled = _dbContext.CourseEnrollment.FirstOrDefault(c => c.CourseId == courseId && c.StudentId == userId);

                if (isEnrolled != null)
                    ViewBag.IsEnrolled = true;
            }

            return View(course);
        }
        /// <summary>
        /// View all the courses a user is enrolled in
        /// </summary>
        /// <returns></returns>
        public IActionResult EnrolledCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var courses = _dbContext.Course
            .Where(c => c.CourseEnrollment.Any(cu => cu.StudentId == userId))
            .ToList();

            return View(courses);
        }


        /// <summary>
        /// Enroll a user into a course and send API request to Finance to create an invoice
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public JsonResult EnrollCourse(int courseId, double courseFee)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var courseEnrollment = new CourseEnrollment()
            {
                CourseId = courseId,
                StudentId = userId,
                EnrollmentDate = DateTime.Now
            };

            _dbContext.CourseEnrollment.Add(courseEnrollment);

            _dbContext.SaveChanges();


            var financeHelper = new FinancePortalHelper();

            //Create Invoice in finance portal for Course fee
            financeHelper.CreateInvoice(GetStudentId(), courseFee);

            return Json(new { data = true });
        }

        private string GetStudentId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.FirstOrDefault(c => c.Id == userId);
            return user.StudentId;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
