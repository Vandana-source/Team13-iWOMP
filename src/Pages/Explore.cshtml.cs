using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
{
    public class ExploreModel : PageModel
    {
        private readonly ILogger<ExploreModel> _logger;

        public string LocationType { get; set; }

        public ExploreModel(ILogger<ExploreModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        public JsonFileProductService ProductService { get; }
        public IEnumerable<ProductModel> Products { get; private set; }

        public void OnGet(string LocationType)
        {
            var allProducts = ProductService.GetProducts();

            // Check if location is not null or empty
            if (!LocationType.Equals("All"))
            {
                // Filter products based on location
                // Leaving it location for now: need to add logic to do others

                Products = allProducts.Where(p => p.LocationType == LocationType);
            }
            else
            {
                // If location is null or empty, return all products
                Products = allProducts.Take(36);
            }
        }

    }
}