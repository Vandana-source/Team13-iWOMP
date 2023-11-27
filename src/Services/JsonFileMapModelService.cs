using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using TakeABreak.WebSite.Models;

namespace TakeABreak.WebSite.Services
{
    public class JsonFileMapModelService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JsonFileMapModelService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        private string JsonFileName
        {
            get
            {
                return Path.Combine(_webHostEnvironment.WebRootPath, "data", "locations.geojson");
            }
        }

        public string GetMapData()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // Read and return the JSON content as a string
                return jsonFileReader.ReadToEnd();
            }
        }

    }
}