using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Services;

namespace TakeABreak.WebSite.Pages
{
    /// <summary>
    /// Razor page model for reading product details
    /// </summary>
    public class SpotlightModel : PageModel
    {
        // Data middletier
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Default Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService"></param>
        public SpotlightModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // The data to show
        public ProductModel Product;

        /// <summary>
        /// REST Get request
        /// </summary>
        /// <param name="id"></param>
        public IActionResult OnGet()
        {
            string id = "seattle-university-rhododendron";
            
            // Fetches the product with the specified ID from the service.
            Product  = ProductService.GetProducts().FirstOrDefault(m => m.Id.Equals(id));

            if (Product == null)
            {
                this.ModelState.AddModelError("OnGet", "Read Onget Error");
                return RedirectToPage("./Index"); 
            }

            return Page();
        }
    }
}