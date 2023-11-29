using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TakeABreak.WebSite.Services;
using TakeABreak.WebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TakeABreak.WebSite.Pages.Product
{
    /// <summary>
    /// Page for creating new product entries.
    /// </summary>
    public class CreateModel : PageModel
    {
        public JsonFileProductService ProductService { get; }
        private readonly IWebHostEnvironment _hostingEnvironment;

        /// <summary>
        /// Constructor for CreateModel.
        /// </summary>
        /// <param name="productService">The JSON product data service.</param>
        /// <param name="hostingEnvironment">The hosting environment service.</param>
        [BindProperty] public ProductModel Product { get; set; }


        public CreateModel(JsonFileProductService productService,
            IWebHostEnvironment hostingEnvironment)
        {
            ProductService = productService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IFormFile UploadedFile { get; set; }

        /// <summary>
        /// Handles HTTP GET requests to the page.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Handles HTTP POST requests when the form is submitted.
        /// </summary>
        /// <returns>An IActionResult representing the response to the POST request.</returns>
        public IActionResult OnPost()
        {
            // Validate the MapURL provided
            if (!IsAllowedMapURL(Product.MapURL))
            {
                // Display an error message and redirect back to the page.
                ModelState.AddModelError("MapURL", "Please provide a valid embed Google Maps URL i.e https://www.google.com/maps/embed?pb=!1m18!1m12!1m3");
                return Page();
            }

            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                string fileExtension = Path.GetExtension(UploadedFile.FileName).ToLower();
                if (IsAllowedImageExtension(fileExtension))
                {
                    // Define the directory based on LocationType
                    string subDirectory = Product.LocationType switch
                    {
                        "Table" => "Tables",
                        "Bench" => "Benches",
                        "Restroom" => "Restrooms",
                        _ => "Others"
                    };

                    // Create a unique filename for the uploaded file
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" +
                                            UploadedFile.FileName;

                    // Get the path for saving the file inside the SiteImages folder, then the desired subdirectory within wwwroot
                    string savePath = Path.Combine(_hostingEnvironment.WebRootPath,
                        "SiteImages", subDirectory, uniqueFileName);

                    // Ensure the directory exists, if not, create it
                    Directory.CreateDirectory(Path.GetDirectoryName(savePath));

                    // Save the uploaded file to the server
                    using var fileStream =
                        new FileStream(savePath, FileMode.Create);
                    UploadedFile
                        .CopyTo(
                            fileStream);

                    // Update the Product.Image property with the relative path to the saved file
                    Product.Image = Path.Combine("/SiteImages", subDirectory, uniqueFileName);

                    // Save the product to the database (or in this case, the JSON file)
                    ProductService.CreateData(Product);

                    // Redirect to a success or product listing page after saving
                    return RedirectToPage("Index");
                }
                else
                {
                    // Display an error message for non-allowed image extensions.
                    ModelState.AddModelError("imageFile", "Please select a valid image file (jpg, jpeg, png, gif, or bmp).");
                }
            }

            // Stay on the same page if no file was uploaded or there was an issue
            return Page();
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