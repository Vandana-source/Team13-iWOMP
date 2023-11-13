using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using TakeABreak.WebSite.Models;  // Ensure this namespace contains CustomerModel

namespace TakeABreak.WebSite.Services
{
    /// <summary>
    /// Service class to handle operations related to CustomerModel
    /// </summary>
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

        // Gets the combined path of the JSON file
        private string JsonFileName
        {
            get
            {
                return Path.Combine(WebHostEnvironment.WebRootPath, "data", "customerDetails.json");
            }
        }

        /// <summary>
        /// Method to read all customer data from the JSON file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerModel> GetCustomers()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<CustomerModel[]>(
                    jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).OrderBy(x => x.CustId);
            }
        }

        /// <summary>
        /// Adds a new customer to the json file database for customer nominations
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(CustomerModel newCustomer)
        {
            // Store the list 
            var customers = GetCustomers().ToList();

            // Get a new ID
            newCustomer.CustId = GetNextCustomerId();

            // Add the new customerId to the list 
            customers.Add(newCustomer);

            // Save 
            SaveCustomers(customers);
        }


        /// <summary>
        /// Saves the new customer nomination to the file 
        /// </summary>
        /// <param name="customers"></param>
        public void SaveCustomers(IEnumerable<CustomerModel> customers)
        {
            using (var outputStream = File.Create(JsonFileName))
            {

                // Write the data to the file 
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

        /// <summary>
        ///  Generates next customer ID from the last one
        /// </summary>
        /// <returns>maxId (int): new customerId</returns>
        public int GetNextCustomerId()
        {
            var customers = GetCustomers();

            // If none exist yet 
            if (!customers.Any())
            {
                // Start from 1 if there are no customers yet
                return 1; 
            }

            // Get the highest and increment by 1 
            int maxId = customers.Max(c => c.CustId);
            return maxId + 1;
        }



    }


}
