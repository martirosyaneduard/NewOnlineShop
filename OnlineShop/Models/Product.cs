using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required,MaxLength(30)]
        public string? Name { get; set; } = String.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public double Weight { get; set; }

        [MaxLength(30)]
        public string? Description { get; set; } = String.Empty;

        [JsonIgnore]
        public List<Order>? Orders { get; set; } = new();
        public int CategoryID { get; set; }

        [JsonIgnore]
        public Category? Category { get; set; }


    }
}
