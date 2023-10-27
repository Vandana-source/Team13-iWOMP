using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
namespace ContosoCrafts.WebSite.Pages.Product
{

    /// <summary>
    /// Create Page
    /// </summary>
    public class CreateModel : PageModel
    {

        // Data middle tier
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Defualt Constructor
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="logger"></param>
        public CreateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // The data to show
        // ProductModel
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// Handles the GET request for the Create page.
        /// Currently, no specific action is required before rendering the view.
        /// This method is kept for clarity and potential future use.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Post the model back to the page
        /// The model is in the class variable Product
        /// Call the data layer to Update that data
        /// Then return to the Index page
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            // Update data into database
            ProductService.CreateData(Product);

            return RedirectToPage("./Index");
        }
    }
}