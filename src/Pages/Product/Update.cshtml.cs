using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TakeABreak.WebSite.Services;
using TakeABreak.WebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static System.Net.WebRequestMethods;
using System.Text.RegularExpressions;

namespace TakeABreak.WebSite.Pages.Product
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
        public IActionResult OnGet(string id)
        {
            Product = ProductService.GetProducts()
                .FirstOrDefault(m => m.Id.Equals(id));
            if (Product == null)
            {
                this.ModelState.AddModelError("OnGet", "Update Onget Error");
                return RedirectToPage("../Error");
            }

            return Page();
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

            if (Product.MapURL != null)
            {
                // Validate the MapURL provided
                if (!IsAllowedMapURL(Product.MapURL))
                {
                    // Display an error message and redirect back to the page.
                    ModelState.AddModelError("MapURL", "Please provide a valid embed Google Maps URL i.e https://www.google.com/maps/embed?pb=!1m18!1m12!1m3");
                    return Page();
                }
            }

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
                string fileExtension = Path.GetExtension(UploadedFile.FileName).ToLower();
                if (!IsAllowedImageExtension(fileExtension))
                {
                    // Display an error message and redirect back to the page.
                    ModelState.AddModelError("imageFile", "Please select a valid image file (jpg, jpeg, png, gif, or bmp).");
                    return Page();
                }
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

        /// <summary>
        /// Method to validate the type of image provided
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsAllowedImageExtension(string extension)
        {
            // List of valid extensions
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

            // If the extension is valid returns true, if not returns false
            return allowedExtensions.Contains(extension);
        }

        /// <summary>
        /// Method to validate the pattern provided for the MapURL
        /// </summary>
        /// <param name="MapURL"></param>
        /// <returns></returns>
        private bool IsAllowedMapURL(string MapURL)
        {
            // Valid pattern for the Maps URL
            string googleMapsPattern = @"^https://www\.google\.com/maps/embed\?pb=!.+$"; // Adjust the pattern as needed

            // Regular expression of the pattern
            Regex regex = new Regex(googleMapsPattern);

            // Return true if the pattern matches, if not returns false
            return regex.IsMatch(MapURL);
        }
    }
}