using System.ComponentModel.DataAnnotations;

namespace WebApiNet.Models.Entities
{
    public class AddressEntity
    {
        public AddressEntity(string street, string zipCode, string city, string country)
        {
            Street = street;
            ZipCode = zipCode;
            City = city;
            Country = country;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public ICollection<CustomerEntity> Customers { get; set;}
    }
}
