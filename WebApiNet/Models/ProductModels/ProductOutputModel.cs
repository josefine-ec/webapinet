namespace WebApiNet.Models.ProductModels
{
    public class ProductOutputModel
    {
        public ProductOutputModel(int id, string product, string description, decimal price, string eAN, CategoryModel category)
        {
            Id = id;
            Product = product;
            Description = description;
            Price = price;
            EAN = eAN;
            Category = category;
        }

        public int Id { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string EAN { get; set; }
        public CategoryModel Category { get; set; }
    }
}
