using Microsoft.AspNetCore.Mvc;
using BookStoreApp.Models;
using BookStoreApp.Services;
using System;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _CourseService;

        public CourseController(ICourseService courseService)
        {
            _CourseService = courseService;

        }

        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            try
            {
                if (course == null)
                {
                    return BadRequest("Invalid Course data");
                }

                var success = _CourseService.AddCourse(course);

                if (success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Failed to add the Course. Please try again.");
                return View(course);
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error response or another appropriate response
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
                return View(course);
            }
        }

        public IActionResult Index()
        {
            try
            {
                var listCourses = _CourseService.GetAllCourses();
                return View("Index",listCourses);
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error view or another appropriate response
                return View("Error"); // Assuming you have an "Error" view
            }
        }

       public IActionResult Delete(int id)
        {
            try
            {
                var success = _CourseService.DeleteCourse(id);

                if (success)
                {
                    // Check if the deletion was successful and return a view or a redirect
                    return RedirectToAction("Index"); // Redirect to the list of Courses, for example
                }
                else
                {
                    // Handle other error cases
                    return View("Error"); // Assuming you have an "Error" view
                }
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);

                // Return an error view or another appropriate response
                return View("Error"); // Assuming you have an "Error" view
            }
        }
    }
}
