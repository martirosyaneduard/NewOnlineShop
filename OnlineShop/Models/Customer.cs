using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Required,MaxLength(30)]
        public string? FirstName { get; set; }=String.Empty;

        [Required, MaxLength(30)]
        public string? LastName { get; set; } = String.Empty;

        [Required,EmailAddress]
        public string? Email { get; set; } = String.Empty;

        [Phone]
        public string? Phone { get; set; } = String.Empty;

        [MaxLength(30)]
        public string? Country { get; set; } = String.Empty;
        public List<Order>? Orders { get; set; } = new();
    }
}
