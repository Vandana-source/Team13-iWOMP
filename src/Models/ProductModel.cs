using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TakeABreak.WebSite.Models
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
        // Validating the string length of Title to be between 5 and 100 characters
        [StringLength(maximumLength: 100, MinimumLength = 5, ErrorMessage = "The Title should have a length of more than {2} and less than {1}")]
        public string Title { get; set; }

        //Validating the string length of Description to be between 25 and 1000 characters
        [StringLength(maximumLength: 1000, MinimumLength = 25, ErrorMessage = "The Description should have a length of more than {2} and less than {1}")]
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
        
        // An integer number representing the noise level 
        public int NoiseLevel { get; set; }

        // Store the Comments entered by the users on this product
        public string[] CommentList { get; set; }
        
        // Overrides the ToString method to serialize the object to JSON.
        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);
    }
}