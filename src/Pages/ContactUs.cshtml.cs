using System;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Services;

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
        [BindProperty]
        public CustomerModel CustomerNomination { get; set; }

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
            // Validate the NominatedMapDetails URL
            if (!IsAllowedMapURL(CustomerNomination.NominatedMapDetails))
            {
                ModelState.AddModelError("NominatedMapDetails", "Please provide a valid Google Maps URL");
                return Page();
            }

            // Assuming the nominated image URL is provided directly in the CustomerNomination object
            if (!string.IsNullOrEmpty(CustomerNomination.NominatedImage))
            {
                // Here you can add any logic needed for the nominated image URL,
                // like validation or formatting.

                // Save the customer data
                CustomerService.AddCustomer(CustomerNomination);

                // Redirect to a success or listing page after saving
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError("NominatedImage", "Please provide a URL for the nominated image.");
                return Page();
            }
        }

        /// <summary>
        /// Validates a Google Maps URL
        /// </summary>
        /// <param name="mapURL">The URL to validate</param>
        /// <returns>True if the URL is valid, otherwise False</returns>
        private bool IsAllowedMapURL(string mapURL)
        {
            string googleMapsPattern = @"^https://www\.google\.com/maps/embed\?pb=!.+$";
            Regex regex = new Regex(googleMapsPattern);
            return regex.IsMatch(mapURL);
        }
    }
}
