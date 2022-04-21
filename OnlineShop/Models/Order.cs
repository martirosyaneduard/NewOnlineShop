using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineShop.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required,MaxLength(50)]
        public string? Status { get; set; } = String.Empty;

        [Required]
        public DateTime CreatedAt { get;}= DateTime.Now;

        public int CustomerID { get; set; }

        [JsonIgnore]
        public Customer? Customer { get; set; }
        public List<Product>? Products { get; set; } = new();

    }
}
