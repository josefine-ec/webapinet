using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet.Models.Entities
{
    public class OrderEntity
    {
        public OrderEntity()
        {

        }
        public OrderEntity(int amount)
        {
            Amount = amount;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        
        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }

        
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
    }
}
