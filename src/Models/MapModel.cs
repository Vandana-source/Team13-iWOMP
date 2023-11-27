using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TakeABreak.WebSite.Models
{
    /// <summary>
    /// Represents a feature on a map, conforming to the GeoJSON standard.
    /// </summary>
    public class MapModel
    {
        // Gets or sets the type of the GeoJSON object, usually "Feature".
        public string Type { get; set; }
       
        // Gets or sets the list of map features.
        public List<MapModelFeature> Features { get; set; }
        
        
        // Overrides the to string method
        public override string ToString() => JsonSerializer.Serialize<MapModel>(this);
        
    }

    /// <summary>
    /// Represents a map feature within a MapModel.
    /// </summary>
    public class MapModelFeature
    {
        // Gets or sets the type of the feature.
        public string Type { get; set; }

        // Gets or sets the properties of the map feature.
        public MapModelProperties Properties { get; set; }

        // Gets or sets the geometry information of the map feature.
        public MapModelGeometry Geometry { get; set; }
    }

    /// <summary>
    /// Represents the properties of a map feature.
    /// </summary>
    public class MapModelProperties
    {
        // Gets or sets the location type of the feature (e.g., "Restroom").
        public string LocationType { get; set; }

        // Gets or sets the unique identifier for the feature.
        public string Id { get; set; }

    }

    /// <summary>
    /// Represents the geometry of a map feature.
    /// </summary>
    public class MapModelGeometry
    {
        // Gets or sets the geometry type (e.g., "Point").
        public string Type { get; set; }

        // Gets or sets the array representing longitude and latitude.
        public double[] Coordinates { get; set; }
    }
}
