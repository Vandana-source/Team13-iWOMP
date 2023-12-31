using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;

using TakeABreak.WebSite.Models;
using TakeABreak.WebSite.Services;

namespace TakeABreak.WebSite.Pages.Product
{
    /// <summary>
    /// Index Page will return all the data to show
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        public IndexModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // Data Service
        public JsonFileProductService ProductService { get; }
        
        // Collection of the Data
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// REST OnGet, return all data
        /// </summary>
        public void OnGet()
        {
            // This method fetches all the data from the JsonFileProductService.cs
            Products = ProductService.GetProducts();
        }
    }
}