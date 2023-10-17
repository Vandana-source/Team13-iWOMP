using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string LocationType { get; set; }

        [JsonPropertyName("img")]
        public string Image { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Neighborhood { get; set; }
        public string MapURL { get; set; }
        public string FunFacts { get; set; }
        public string Address {get; set;}
        public int[] Ratings {get; set; }

        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);

 
    }
}