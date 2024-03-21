using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentPortal.BL;
using StudentPortal.Data;
using StudentPortal.Models;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentPortal.Controllers
{
    public class StudentController : Controller
    {

        private ApplicationDbContext _dbContext;
        private readonly IConfigurationRoot configuration;

        /// <summary>
        /// Overloaded constructor for Dependency Injection
        /// </summary>
        /// <param name="dbContext"></param>
        public StudentController(ApplicationDbContext dbContext, IConfigurationBuilder builder)
        {
            _dbContext = dbContext;

            configuration = builder.AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();
        }

        public IActionResult GraduationEligibility()
        {
            var studentId = GetStudentId();
            var financeHelper = new FinancePortalHelper();

            var result = financeHelper.CheckGraduationEligibility(studentId);

            ViewBag.Eligibility = result;

            return View();
        }

        public IActionResult InvoiceList()
        {
            var studentId = GetStudentId();
            var financeHelper = new FinancePortalHelper();

            var invoices = financeHelper.GetAllStudentInvoice(studentId);
            ViewBag.FinanceAppUrl = configuration.GetValue<string>("FinanceAppUrl");
            return View(invoices);
        }

        /// <summary>
        /// Get the logged in user profile
        /// </summary>
        /// <returns></returns>
        public IActionResult Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.FirstOrDefault(c => c.Id == userId);
            var md = new Profile()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                StudentId = user.StudentId,
                Email = user.Email
            };
            return View(md);
        }

        /// <summary>
        /// Edit logged in user profile
        /// </summary>
        /// <returns></returns>
        public IActionResult EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.FirstOrDefault(c => c.Id == userId);

            var md = new Profile()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                StudentId = user.StudentId,
                Email = user.Email
            };
            return View(md);
        }

        [HttpPost]
        public IActionResult EditProfile(Profile model)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
       .SelectMany(v => v.Errors)
       .Select(e => e.ErrorMessage));
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _dbContext.Set<ApplicationUser>().Find(userId);


            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Profile));
        }

        private string GetStudentId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.FirstOrDefault(c => c.Id == userId);
            return user.StudentId;
        }
    }
}
