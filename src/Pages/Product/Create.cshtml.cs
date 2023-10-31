using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateModel : PageModel
    {
        public JsonFileProductService ProductService { get; }
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty] public ProductModel Product { get; set; }
        [BindProperty] public IFormFile UploadedImage { get; set; }

        public CreateModel(JsonFileProductService productService,
            IWebHostEnvironment hostingEnvironment)
        {
            ProductService = productService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IFormFile UploadedFile { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
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
                        fileStream); // Note: Using the synchronous method here

                // Update the Product.Image property with the relative path to the saved file
                Product.Image = Path.Combine("/SiteImages", subDirectory, uniqueFileName);

                // Save the product to the database (or in this case, the JSON file)
                ProductService.CreateData(Product);

                // Redirect to a success or product listing page after saving
                return RedirectToPage("Index"); // Assuming you have an Index page for listing products
            }

            // Stay on the same page if no file was uploaded or there was an issue
            return Page();
        }
    }
}