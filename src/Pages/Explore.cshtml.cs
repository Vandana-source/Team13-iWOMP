using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary>
    /// Explore Page will return all the data to show according to filter
    /// </summary>
    public class ExploreModel : PageModel
    {
        //Logger 
        private readonly ILogger<ExploreModel> _logger;

        //Location Type of product
        public string LocationType { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        public ExploreModel(ILogger<ExploreModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        // Data Service
        public JsonFileProductService ProductService { get; }

        //Collection of products
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// REST OnGet, return all data based on the locationtype 
        /// </summary>
        public void OnGet(string LocationType)
        {
            var allProducts = ProductService.GetProducts();

            // Check if location type is All or not
            if (!LocationType.Equals("All"))
            {
                // Filter products based on location
                Products = allProducts.Where(p => p.LocationType == LocationType);
            }
            else
            {
                // Returns all products
                Products = allProducts.Take(36);
            }
        }

    }
}