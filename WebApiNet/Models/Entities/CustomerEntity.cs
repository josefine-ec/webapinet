using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiNet.Models.Entities
{
    [Index(nameof(Email), IsUnique = true)]

    public class CustomerEntity
    {
        public CustomerEntity()
        {

        }
        

        public CustomerEntity(int id)
        {
            Id = id;
        }

        public CustomerEntity(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public int AddressId { get; set; }
        public AddressEntity Address { get; set; }
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
