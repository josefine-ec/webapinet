namespace WebApiNet.Models.ProductModels
{
    public class ProductUpdate
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
