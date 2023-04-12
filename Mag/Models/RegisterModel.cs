#nullable disable
using System.ComponentModel.DataAnnotations;

namespace Mag.Models
{
    public record RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,32}$", ErrorMessage = "Password should contain: <br> at least one digit appear anywhere in the string <br> at least one lowercase letter appear anywhere in the string <br> at least one uppercase letter appear anywhere in the string <br> The password must be at least 6 characters long, but no more than 32")]
        public string Password { get; set; }
    }
}
