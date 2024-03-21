using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using StudentPortal.BL;
using StudentPortal.Controllers;
using StudentPortal.Data;
using StudentPortal.Models;
using System.Security.Claims;

namespace StudentTest
{
    public class CourseTest
    {
        [Fact]
        public void Courses_ReturnsViewResultWithListOfCourses()
        {
            // Arrange
            var courses = new List<Course>
            {
                new Course { CourseId = 1, Title = "Course 1", Description = "Description" },
                new Course { CourseId = 2, Title = "Course 2", Description = "Description" }
            };
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
            var mockDbContext = new Mock<ApplicationDbContext>(options);
            mockDbContext.Setup(c => c.Course).Returns(MockDbSet(courses.AsQueryable()));

            var controller = new CourseController(mockDbContext.Object);

            // Act
            var result = controller.Courses() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Course>>(result.Model);

            var model = result.Model as List<Course>;
            Assert.Equal(2, model.Count); // Assuming there are 2 courses returned
        }


        [Fact]
        public void CourseDetail_WithInvalidCourseId_ShouldReturnNotFoundResult()
        {
            // Arrange
            var courseId = 1;

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().Options;
            var mockDbContext = new Mock<ApplicationDbContext>(options);
            mockDbContext.Setup(c => c.Course).Returns(MockDbSet<Course>(new List<Course>().AsQueryable()));

            var controller = new CourseController(mockDbContext.Object);

            // Act
            var result = controller.CourseDetail(courseId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        // Helper method to mock DbSet
        private static DbSet<T> MockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }
    }
}