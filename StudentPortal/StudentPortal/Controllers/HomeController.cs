using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StudentPortal.BL;
using StudentPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentPortal.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext;

        public HomeController()
        {

        }

        /// <summary>
        /// Overloaded constructor for Dependency Injection
        /// </summary>
        /// <param name="dbContext"></param>
        public HomeController(ApplicationDbContext dbContext)
        {
            ApplicationDbContext = dbContext;
        }


        /// <summary>
        /// Providing/Injecting dependency into constructor
        /// </summary>
        public ApplicationDbContext ApplicationDbContext
        {
            get
            {
                return _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _dbContext = value;
            }
        }

        /// <summary>
        /// List all the courses from Database
        /// </summary>
        /// <returns></returns>
        public ActionResult Courses()
        {
            var courses = ApplicationDbContext.Course.ToList();
            return View(courses);
        }

        /// <summary>
        /// View the detail of a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public ActionResult CourseDetail(int courseId)
        {
            var course = ApplicationDbContext.Course.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return HttpNotFound();

            if(User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var isEnrolled = ApplicationDbContext.CourseEnrollment.FirstOrDefault(c => c.CourseId == courseId && c.StudentId == userId);

                if (isEnrolled != null)
                    ViewBag.IsEnrolled = true;
            }

            return View(course);
        }


        /// <summary>
        /// View all the courses a user is enrolled in
        /// </summary>
        /// <returns></returns>
        public ActionResult EnrolledCourses()
        {
            var userId = User.Identity.GetUserId();

            var courses = ApplicationDbContext.Course
            .Where(c => c.CourseEnrollment.Any(cu => cu.StudentId == userId))
            .ToList();

            return View(courses);
        }


        /// <summary>
        /// Enroll a user into a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public JsonResult EnrollCourse(int courseId, double courseFee)
        {
            var courseEnrollment = new CourseEnrollment()
            {
                CourseId = courseId,
                StudentId = User.Identity.GetUserId(),
                EnrollmentDate = DateTime.Now
            };

            ApplicationDbContext.CourseEnrollment.Add(courseEnrollment);

            ApplicationDbContext.SaveChanges();

            var financeHelper = new FinancePortalHelper();

            var userId = User.Identity.GetUserId();

            //Create Invoice in finance portal for Course fee
            financeHelper.CreateInvoice(userId, courseFee);

            return Json(new { data = true });
        }
    }
}