using WebApiNet.Models.Entities;

namespace WebApiNet.Models.OrderModels
{
    public class OrderOutputModel
    {
        public OrderOutputModel()
        {

        }
        public OrderOutputModel(int id, DateTime createdTime, int amount, CustomerEntity customer, ProductEntity product)
        {
            Id = id;
            CreatedTime = createdTime;
            Amount = amount;
            Customer = customer;
            Product = product;
        }

        public int Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public int Amount { get; set; }

        public CustomerEntity Customer { get; set; }
        public ProductEntity Product { get; set; }
    }
}
