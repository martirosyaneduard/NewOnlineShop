using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineShop.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required,MaxLength(30)]
        public string? Name { get; set; } = String.Empty;
        public List<Product>? Products { get; set; } = new();
    }
}
