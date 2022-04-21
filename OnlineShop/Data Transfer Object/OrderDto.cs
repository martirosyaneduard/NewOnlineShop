using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Data_Transfer_Object
{
    public class OrderDto
    {
        [Required, MaxLength(50)]
        public string? Status { get; set; } = String.Empty;
        public int CustomerId { get; set; }
        public List<int> ProductsId { get; set; }

    }
}
