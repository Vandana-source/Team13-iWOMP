using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Represents the code-behind for the ContactUs Razor Page.
    /// </summary>
    public class ExploreModel : PageModel
    {
        // Declare a private field to hold a logger instance
        private readonly ILogger<ExploreModel> _logger;

        // Constructor for ExploreModel, takes a logger as a dependency
        public ExploreModel(ILogger<ExploreModel> logger,
            JsonFileProductService productService)
        {
            // Initiate the logger field with the injected logger
            _logger = logger;
            ProductService = productService;
        }

        public JsonFileProductService ProductService { get; }
        public IEnumerable<ProductModel> Products { get; private set; }

        public void OnGet()
        {
            // This method fetches all the data from the JsonFileProductService.cs
            Products = ProductService.GetProducts();
        }
    }
}