using System;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Services;
using System.Threading;

namespace TakeABreak.WebSite.Pages
{
    /// <summary>
    /// Page for adding customer nominations/contact details 
    /// </summary>
    public class ContactUsModel : PageModel
    {
        // Service to handle customer data operations
        public JsonFileCustomerService CustomerService { get; }

        // Web hosting environment for file operations
        private readonly IWebHostEnvironment _hostingEnvironment;

        // Logger for logging messages
        private readonly ILogger<ContactUsModel> _logger;

        // Model for customer nomination data binding
        [BindProperty] public CustomerModel CustomerNomination { get; set; }

        /// <summary>
        /// Constructor for ContactUsModel
        /// </summary>
        /// <param name="customerService">The customer service</param>
        /// <param name="hostingEnvironment">The web hosting environment</param>
        /// <param name="logger">The logger</param>
        public ContactUsModel(JsonFileCustomerService customerService,
            IWebHostEnvironment hostingEnvironment,
            ILogger<ContactUsModel> logger)
        {
            CustomerService = customerService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        /// <summary>
        /// Handles HTTP GET request for the page
        /// </summary>
        public void OnGet()
        {
            // PageModel's method for HTTP GET request handling.
        }

        /// <summary>
        /// Handles HTTP POST request for the page
        /// </summary>
        /// <returns>ActionResult</returns>
        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                // Add to the database 
                // Note: User data minimally vetted because it is not appearing on
                //       the website immediately and goes to the database.  
                CustomerService.AddCustomer(CustomerNomination);

                // Thank the user for their submission 

                TempData["SubmissionMessage"] = "Thank you for your submission";

                return RedirectToPage("/ContactUs");
                // Stay on the page
            }
            catch
            {
                return RedirectToPage("../Error");
            }
        }
    }
}