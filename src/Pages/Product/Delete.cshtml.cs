using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;

using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace TakeABreak.WebSite.Pages.Product

{
    /// <summary>
    /// Delete Page will delete the data from json file
    /// </summary>
    public class DeleteModel : PageModel
    {
        //Data Service
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        public DeleteModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // Collection of the Data
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// REST OnGet, return all data
        /// </summary>
        public IActionResult OnGet(string id)
        {
            // This method fetches all the data from the JsonFileProductService.cs
            Product = ProductService.GetProducts().FirstOrDefault(m => m.Id.Equals(id));
            if (Product == null)
            {
                this.ModelState.AddModelError("OnGet", "Update Onget Error");
                return RedirectToPage("../Error");
            }
            return Page();

        }

        /// <summary>
        /// OnPost Method, will get the product which should be deleted from DB
        /// </summary>
        public IActionResult OnPost()
        {
            // This method will check if the Model is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // This method will call the JsonFileProductService.cs Delete method which will delete the product
            ProductService.DeleteData(Product);
            // This will redirect to the Index page once the data is deleted.
            return RedirectToPage("./Index");
        }
    }
}