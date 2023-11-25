using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TakeABreak.WebSite.Models
{
    /// <summary>
    /// Represents a Map feature, including its type, properties, and geometry.
    /// </summary>
    public class MapModelFeature
    {
        // Defines the type of the Map object, typically "Feature"
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Feature";

        // Holds the descriptive properties of the feature, such as name and productID
        [JsonPropertyName("properties")]
        public MapModelProperties Properties { get; set; }

        // Contains the geometry of the feature, including type and coordinates
        [JsonPropertyName("geometry")]
        public MapModelGeometry Geometry { get; set; }

        /// <summary>
        /// Serializes the MapModelFeature object to a JSON string with indentation.
        /// </summary>
        /// <returns>A JSON string representing the MapModelFeature object.</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize<MapModelFeature>(this);

        }
    }

    /// <summary>
    /// Contains the properties of a Map feature, like name and productID.
    /// </summary>
    public class MapModelProperties
    {
        // The name or title of the Map feature
        [JsonPropertyName("name")]
        public string Name { get; set; }

        // A unique identifier, such as a productID, associated with the feature
        [JsonPropertyName("productID")]
        public string ProductID { get; set; }

        // Add other properties as needed
    }

    /// <summary>
    /// Describes the geometry of a Map feature, including its type and coordinates.
    /// </summary>
    public class MapModelGeometry
    {
        // Specifies the geometry type, e.g., "Point", "LineString", "Polygon"
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Point";

        // An array of coordinates defining the geometry, format varies with the type
        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; }
    }
}
