using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TakeABreak.WebSite.Pages
{
    /// <summary>
    ///  Represents the error page model for the TakeABreak website.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        // Gets or dets the id of the current request
        public string RequestId { get; set; }

        // Determines whether to show the RequestId or not
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Logger instance to log error details
        private readonly ILogger<ErrorModel> _logger;


        /// <summary>
        /// Initializes a new instance of the ErrorModel class
        /// </summary>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            // Instsance for error logging details 
            _logger = logger;
        }

        /// <summary>
        /// Method executed on GET request to the error page.
        /// </summary>
        public void OnGet()
        {
            // Sets the RequestId property.
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}