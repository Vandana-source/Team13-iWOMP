using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Represents the UpdateModel for the products in the TakeABreak website.
    /// This model is responsible for handling the update operations for products.
    /// </summary>
    public class UpdateModel : PageModel
    {
        // Data middletier
        // Service to handle product data operations
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Defualt Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService"></param>
        public UpdateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        //// <summary>
        /// Constructor that initializes the UpdateModel with the required services.
        /// </summary>
        [BindProperty]
        public ProductModel Product { get; set; }


        /// <summary>
        /// Method executed on GET request to the Update page.
        /// Retrieves the product data based on the provided id.
        /// <param name="id">the string identifier of product to fetch</param>
        /// </summary>
        public void OnGet(string id)
        {
            Product = ProductService.GetProducts().FirstOrDefault(m => m.Id.Equals(id));
        }

        /// <summary>
        /// Method executed on POST request to the Update page.
        /// If the model is valid, updates the product data.
        /// </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ProductService.UpdateData(Product);

            return RedirectToPage("./Index");
        }
    }
}