using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// In project a 'product model' is a physical location and this describes its attributes
    /// </summary>
    public class ProductModel
    {
        // Unique identifier for the product.
        public string Id { get; set; }

        // Type of location.
        public string LocationType { get; set; }

        // JSON property name mapping for 'img' to 'Image'.
        [JsonPropertyName("img")]
        public string Image { get; set;}

        // URL of the product.
        public string Url { get; set; }

        // Title of the product.
        public string Title { get; set; }

        // Description of the product.
        public string Description { get; set; }

        // Neighborhood where the product is located.
        public string Neighborhood { get; set; }

        // URL to the map showing the product's location.
        public string MapURL { get; set; }

        // Fun facts about the product.
        public string FunFacts { get; set; }

        // Address of the product's location.
        public string Address {get; set; }

        // Array to store ratings for the product.
        public int[] Ratings {get; set; }

        // Accessibility information for the product's location.
        public string Accessibility { get; set; }

        // Opening hours of the product.
        public string OpeningHours { get; set; }

        // Date and time when the product information was last updated.
        public DateTime LastUpdated { get; set; }

        // Overrides the ToString method to serialize the object to JSON.
        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);

 
    }
}