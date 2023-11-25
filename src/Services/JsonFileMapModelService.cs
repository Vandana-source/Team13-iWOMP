using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using TakeABreak.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace TakeABreak.WebSite.Services
{
    /// <summary>
    /// Service class to handle operations related to MapModelFeature.
    /// Provides functionality to read, add, update, and delete map features from a JSON file.
    /// </summary>
    public class JsonFileMapService
    {
        /// <summary>
        /// Initializes a new instance of the JsonFileMapService with the specified web hosting environment.
        /// </summary>
        /// <param name="webHostEnvironment">The web hosting environment.</param>
        public JsonFileMapService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        // Property to access the web hosting environment
        public IWebHostEnvironment WebHostEnvironment { get; }

        /// <summary>
        /// Gets the file path for the JSON data file containing map features.
        /// </summary>
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "maps.json"); }
        }

        /// <summary>
        /// Retrieves all map features from the JSON data file.
        /// </summary>
        /// <returns>A collection of MapModelFeature objects.</returns>
        public IEnumerable<MapModelFeature> GetMapFeatures()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<MapModelFeature[]>(
                    jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        /// <summary>
        /// Adds a new map feature to the JSON data file.
        /// </summary>
        /// <param name="mapFeature">The map feature to add.</param>
        public void AddMapFeature(MapModelFeature mapFeature)
        {
            var mapFeatures = GetMapFeatures().ToList();
            mapFeatures.Add(mapFeature);
            SaveData(mapFeatures);
        }

        /// <summary>
        /// Updates an existing map feature in the JSON data file.
        /// </summary>
        /// <param name="mapFeature">The map feature to update.</param>
        /// <returns>The updated MapModelFeature object.</returns>
        public MapModelFeature UpdateMapFeature(MapModelFeature mapFeature)
        {
            var mapFeatures = GetMapFeatures().ToList();
            var index = mapFeatures.FindIndex(m => m.Properties.Id == mapFeature.Properties.Id);
            if (index != -1)
            {
                mapFeatures[index] = mapFeature;
            }
            SaveData(mapFeatures);
            return mapFeature;
        }

        /// <summary>
        /// Deletes a map feature from the JSON data file based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the map feature to delete.</param>
        public void DeleteMapFeature(string id)
        {
            var mapFeatures = GetMapFeatures().ToList();
            mapFeatures.RemoveAll(m => m.Properties.Id == id);
            SaveData(mapFeatures);
        }

        /// <summary>
        /// Saves the updated collection of map features back to the JSON data file.
        /// </summary>
        /// <param name="mapFeatures">The collection of MapModelFeature objects to save.</param>
        private void SaveData(IEnumerable<MapModelFeature> mapFeatures)
        {
            using (var outputStream = File.Create(JsonFileName))
            {
                JsonSerializer.Serialize(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions { Indented = true }),
                    mapFeatures);
            }
        }
    }
}
