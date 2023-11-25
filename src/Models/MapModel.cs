using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TakeABreak.WebSite.Models
{
    /// <summary>
    /// Represents a feature on a map, conforming to the GeoJSON standard.
    /// </summary>
    public class MapModelFeature
    {
        // Specifies the type of the GeoJSON object, usually "Feature"
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Feature";

        // Contains descriptive properties of the feature, like location type and ID
        [JsonPropertyName("properties")]
        public MapModelProperties Properties { get; set; }

        // Defines the geometry of the feature, such as type and coordinates
        [JsonPropertyName("geometry")]
        public MapModelGeometry Geometry { get; set; }

        /// <summary>
        /// Converts the object to its JSON string representation.
        /// </summary>
        /// <returns>JSON string representing the MapModelFeature object.</returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }

    /// <summary>
    /// Contains the properties of a map feature, like location type and ID.
    /// </summary>
    public class MapModelProperties
    {
        // Defines the location type of the feature (e.g., "Restroom")
        [JsonPropertyName("LocationType")]
        public string LocationType { get; set; }

        // Unique identifier for the feature (e.g., "cal-anderson-park")
        [JsonPropertyName("Id")]
        public string Id { get; set; }
    }

    /// <summary>
    /// Describes the geometry of a map feature, including its type and coordinates.
    /// </summary>
    public class MapModelGeometry
    {
        // Specifies the geometry type (e.g., "Point")
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Point";

        // An array representing the coordinates (longitude, latitude) of the feature
        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; }
    }
}
