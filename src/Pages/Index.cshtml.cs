using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Services;

namespace TakeABreak.WebSite.Pages
{
    /// <summary>
    /// Represents the code-behind for the Index Razor Page.
    /// </summary>
    public class IndexModel : PageModel
    {
        // Declare a private field to hold a logger instance
        private readonly ILogger<IndexModel> _logger;

        // Constructor for IndexModel, takes a logger as a dependency
        public IndexModel(ILogger<IndexModel> logger)
        {
            // Initiate the logger field with the injected logger
            _logger = logger;
        }

        public void OnGet()
        {
            // This is the IndexeModel's method called when an HTTP GET request is made to this page.
            // It is typically used to prepare data for display, but in this case, it's empty, so no specific action is taken.
        }
    }
}