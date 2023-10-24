namespace UnitTests.Pages.Product.Update
{
    internal class ProductModel : ContosoCrafts.WebSite.Models.ProductModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
    }
}