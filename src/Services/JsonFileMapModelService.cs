using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using TakeABreak.WebSite.Models;

namespace TakeABreak.WebSite.Services
{
    /// <summary>
    /// This class provides services for reading JSON data from a file. 
    /// It is specifically designed to work within an ASP.NET Core web application.
    /// </summary>
    public class JsonFileMapModelService
    {
        // The IWebHostEnvironment interface is used to get the details of the web hosting environment
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Constructor to initialize the JsonFileMapModelService with the web host environment.
        /// </summary>
        /// <param name="webHostEnvironment">Provides information about the web hosting environment.</param>
        public JsonFileMapModelService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Property to get the full path of the JSON file.
        /// </summary>
        private string JsonFileName
        {
            get
            {
                // Combines the web root path with the 'data' directory and the 'locations.geojson' file name
                return Path.Combine(_webHostEnvironment.WebRootPath, "data", "locations.geojson");
            }
        }

        /// <summary>
        /// Reads and returns the content of the JSON file as a string.
        /// </summary>
        /// <returns>A string containing the JSON data from the file.</returns>
        public string GetMapData()
        {
            // Opens the JSON file for reading
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // Read and return the JSON content as a string
                return jsonFileReader.ReadToEnd();
            }
        }

    }
}