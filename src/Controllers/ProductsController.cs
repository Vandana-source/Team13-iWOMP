using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Controllers
{
    
    /// <summary>
    /// Get the root path for ProductsController file
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        /// <summary>
        /// Constructor for ProductsController
        /// </summary>
        /// <param name="productService"></param>
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        // Getter for ProductService
        public JsonFileProductService ProductService { get; }

        
        /// <summary>
        /// GetProducts method is used to get the list of all the products, it parses the JSON file and converts into products model list
        /// </summary>
        /// <returns>List of Product Model</returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            return ProductService.GetProducts();
        }

        
        /// <summary>
        /// Add rating to specific product selected
        /// </summary>
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {
            // Adding rating
            ProductService.AddRating(request.ProductId, request.Rating);
            
            return Ok();
        }

        /// <summary>
        /// Get Rating request by product Id
        /// </summary>
        public class RatingRequest
        {
            /// <summary>
            /// Getter and Setter for ProductId
            /// </summary>
            public string ProductId { get; set; }

            /// <summary>
            /// Getter and Setter for Rating
            /// </summary>
            public int Rating { get; set; }
        }
    }
}