using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Represents the code-behind for the Privacy Razor Page.
    /// </summary>
    public class PrivacyModel : PageModel
    {
        // Declare a private field to hold a logger instance
        private readonly ILogger<PrivacyModel> _logger;

        // Constructor for PrivacyModel, takes a logger as a dependency
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            // Initialize the logger field with the injected logger
            _logger = logger;
        }

        public void OnGet()
        {
            // This is the PageModel's method called when an HTTP GET request is made to this page.
            // It is typically used to prepare data for display, but in this case, it's empty, so no specific action is taken.
        }
    }
}