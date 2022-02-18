namespace WebApiNet.Models.OrderModels
{
    public class OrderInputModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public int CustomerId { get; set; }
    }
}
