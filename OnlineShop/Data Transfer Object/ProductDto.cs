using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Data_Transfer_Object
{
    public class ProductDto
    {
        [Required, MaxLength(30)]
        public string? Name { get; set; } = String.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public double Weight { get; set; }

        [MaxLength(30)]
        public string? Description { get; set; } = String.Empty;

        public int CategoryID { get; set; }
    }
}
