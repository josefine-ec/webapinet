namespace WebApiNet.Models.ProductModels
{
    public class ProductInputModel
    {
        public string Category { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public string EAN { get; set; }
        public decimal Price { get; set; }
    }
}
