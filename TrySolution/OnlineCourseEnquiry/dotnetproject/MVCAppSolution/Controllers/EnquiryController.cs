using Microsoft.AspNetCore.Mvc;
using BookStoreApp.Models;
using BookStoreApp.Services;
using System;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    public class EnquiryController : Controller
    {
        private readonly IEnquiryService _EnquiryService;

        public EnquiryController(IEnquiryService enquiryService)
        {
            _EnquiryService = enquiryService;

        }

        public IActionResult AddEnquiry()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEnquiry(Enquiry enquiry)
        {
            try
            {
                if (enquiry == null)
                {
                    return BadRequest("Invalid Enquiry data");
                }

                var success = _EnquiryService.AddEnquiry(enquiry);

                if (success)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Failed to add the Enquiry. Please try again.");
                return View(enquiry);
            }
            catch (Exception ex)
            {
                // Log or print the exception to get more details
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                // Return an error response or another appropriate response
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again.");
                return View(enquiry);
            }
        }

        public IActionResult Index()
        {
            try
            {
                var listEnquirys = _EnquiryService.GetAllEnquirys();
                return View("Index", listEnquirys);
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
                var success = _EnquiryService.DeleteEnquiry(id);

                if (success)
                {
                    // Check if the deletion was successful and return a view or a redirect
                    return RedirectToAction("Index"); // Redirect to the list of Enquirys, for example
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
