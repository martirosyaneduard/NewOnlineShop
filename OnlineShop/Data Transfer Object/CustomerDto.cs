using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Data_Transfer_Object
{
    public class CustomerDto
    {
        [Required, MaxLength(10)]
        public string? FirstName { get; set; } = String.Empty;

        [Required, MaxLength(30)]
        public string? LastName { get; set; } = String.Empty;

        [Required, EmailAddress]
        public string? Email { get; set; } = String.Empty;

        [Phone]
        public string? Phone { get; set; } = String.Empty;

        [MaxLength(30)]
        public string? Country { get; set; } = String.Empty;
    }
}
