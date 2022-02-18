using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet.Models.Entities
{
    public class ProductEntity
    {
        

        public ProductEntity(int id)
        {
            Id = id;
        }

        public ProductEntity(string eAN, string product, string description, decimal price)
        {
            EAN = eAN;
            Product = product;
            Description = description;
            Price = price;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string EAN { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
