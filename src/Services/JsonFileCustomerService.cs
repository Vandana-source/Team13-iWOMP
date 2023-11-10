using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using TakeABreak.WebSite.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace TakeABreak.WebSite.Services
{
    public class JsonFileCustomerService
    {
        
        /// <summary>
        /// Setting the web hosting environment
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        public JsonFileCustomerService(IWebHostEnvironment webHostEnvironment)
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
                    "contactDetails.json");
            }
        }
        
        /// <summary>
        /// Save all customer data to storage
        /// </summary>
        /// <param name="customers"></param>
        private void SaveData(IEnumerable<CustomerModel> customers)
        {
            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize<IEnumerable<CustomerModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    customers
                );
            }
        }
    }
}