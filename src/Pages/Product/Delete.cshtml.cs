using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContosoCrafts.WebSite.Pages.Product

{
    public class DeleteModel : PageModel
    {
        public JsonFileProductService ProductService { get; }
        
        public DeleteModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        [BindProperty]
        public ProductModel Product { get; set; }

        public void OnGet(string id)
        {
            Product = ProductService.GetProducts().FirstOrDefault(m => m.Id.Equals(id));
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ProductService.DeleteData(Product);

            
        }
    }

}