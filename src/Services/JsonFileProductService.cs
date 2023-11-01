using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Service class write to handle operations related to ProductModel
    /// </summary>
    public class JsonFileProductService
    {
        /// <summary>
        /// Setting the web hosting environment
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // Assigned IWebHostEnvironment to the public property
        public IWebHostEnvironment WebHostEnvironment { get; }

        // Gets the combined path of JSON file
        private string JsonFileName
        {
            get
            {
                return Path.Combine(WebHostEnvironment.WebRootPath, "data",
                    "products.json");
            }
        }

        /// <summary>
        /// REST call to read all data from the JSON files
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductModel> GetProducts()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<ProductModel[]>(
                    jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).OrderBy(x => x.Id);
            }
        }

        /// <summary>
        /// Add rating
        /// Take in the product ID and the rating
        /// If the rating does not exist, add it
        /// Save the update
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        public bool AddRating(string productId, int rating)
        {
            // If the ProductID is invalid, return
            if (string.IsNullOrEmpty(productId))
            {
                return false;
            }

            var products = GetProducts();

            // Look up the product, if it does not exist, return
            var data = products.FirstOrDefault(x => x.Id.Equals(productId));
            if (data == null)
            {
                return false;
            }

            // Check Rating for boundries, do not allow ratings below 0
            if (rating < 0)
            {
                return false;
            }

            // Check Rating for boundries, do not allow ratings above 5
            if (rating > 5)
            {
                return false;
            }

            // Check to see if the rating exist, if there are none, then create the array
            if (data.Ratings == null)
            {
                data.Ratings = new int[] { };
            }

            // Add the Rating to the Array
            var ratings = data.Ratings.ToList();
            ratings.Add(rating);
            data.Ratings = ratings.ToArray();

            // Save the data back to the data store
            SaveData(products);

            return true;
        }

        /// <summary>
        /// Find the data record
        /// Update the fields
        /// Save to the data store
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ProductModel UpdateData(ProductModel data)
        {
            var products = GetProducts();
            var productData = products.FirstOrDefault(x => x.Id.Equals(data.Id));

            // Update the data to the new passed in values
            productData.Title = data.Title;
            productData.LocationType = data.LocationType;
            productData.Neighborhood = data.Neighborhood;
            productData.Description = data.Description;
            productData.MapURL = data.MapURL;
            productData.Image = data.Image;
            productData.NoiseLevel = data.NoiseLevel;

            SaveData(products);

            return productData;

        }

        /// <summary>
        /// Save all products data to storage
        /// </summary>
        /// <param name="products"></param>
        private void SaveData(IEnumerable<ProductModel> products)
        {
            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );
            }
        }

        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns></returns>
        public ProductModel CreateData(ProductModel productModel)
        {
            productModel.Id = System.Guid.NewGuid().ToString();

            // Get the current set, and append the new record to it becuase IEnumerable does not have Add
            var dataSet = GetProducts();
            dataSet = dataSet.Append(productModel);

            // Update the products.json with newly created products
            SaveData(dataSet);

            //Return newly created data
            return productModel;
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        public ProductModel DeleteData(ProductModel productModel)
        {
            // Get the current set, and append the new record to it
            var products = GetProducts();
            var productData =
                products.FirstOrDefault(x => x.Id.Equals(productModel.Id));
            
            // Exclude chosen ID from new list 
            var newDataSet = 
                GetProducts().Where(x => x.Id.Equals(productModel.Id) == false);
            
            // Save list without the product 
            SaveData(newDataSet);
           
            return productData; 
            
        }


    }

}