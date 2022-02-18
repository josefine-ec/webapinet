using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Models.Entities
{
    public class CategoryEntity
    {
        public CategoryEntity(string category)
        {
            Category = category;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Category { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
    }
}
