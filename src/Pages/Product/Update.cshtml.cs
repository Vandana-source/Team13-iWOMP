using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Represents the UpdateModel for the products in the TakeABreak website.
    /// This model is responsible for handling the update operations for products.
    /// </summary>
    public class UpdateModel : PageModel
    {
        public JsonFileProductService ProductService { get; }
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty] public ProductModel Product { get; set; }

        [BindProperty] public IFormFile UploadedFile { get; set; }

        public UpdateModel(JsonFileProductService productService,
            IWebHostEnvironment hostingEnvironment)
        {
            ProductService = productService;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Method executed on GET request to the Update page.
        /// Retrieves the product data based on the provided id.
        /// <param name="id">the string identifier of product to fetch</param>
        /// </summary>
        public void OnGet(string id)
        {
            Product = ProductService.GetProducts()
                .FirstOrDefault(m => m.Id.Equals(id));
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
            // To get the old filepath before replacing for later deletion 
            var existingProduct = ProductService.GetProducts()
                .FirstOrDefault(m => m.Id.Equals(Product.Id));
            if (Product.Image == null)
            {
                // Capture the old image path right after fetching the product
                Product.Image = existingProduct.Image;
            }
            // Capture the old image path right after fetching the product
            string oldImagePath = existingProduct.Image;

            // If the uploaded file works: save 
            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                // Define the directory based on LocationType
                string subDirectory = Product.LocationType switch
                {
                    "Table" => "Tables",
                    "Bench" => "Benches",
                    "Restroom" => "Restrooms",
                    _ => "Others"
                };

                // Builds the correct file path based on LocationType
                string uniqueFileName = Guid.NewGuid().ToString() + "_" +
                                        UploadedFile.FileName;
                string savePath = Path.Combine(_hostingEnvironment.WebRootPath,
                    "SiteImages", subDirectory, uniqueFileName);

                // Creates a directory if none exists 
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                // Try to delete the old image using the captured path
                if (!string.IsNullOrEmpty(oldImagePath))
                {
                    string fullPath = Path.Combine(
                        _hostingEnvironment.WebRootPath,
                        oldImagePath.TrimStart('/'));
                    FileInfo fileInfo = new FileInfo(fullPath);

                    if (fileInfo.Exists)
                    {
                        // Delete the file 
                        fileInfo.Delete();
                    }

                }

                // Creates a new filepath and saves it to the file 
                using var fileStream =
                    new FileStream(savePath, FileMode.Create);
                UploadedFile.CopyTo(fileStream);

                Product.Image = Path.Combine("/SiteImages", subDirectory,
                    uniqueFileName);
                ProductService.UpdateData(Product);

                // Returns to the index page 
                return RedirectToPage("./Index");
            }

            // Saves the updated data & return to index 
            ProductService.UpdateData(Product);
            return RedirectToPage("./Index");
        }
    }
}

