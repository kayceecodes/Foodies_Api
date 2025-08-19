using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Requests;

public class LoginRequest
{
        [Required, RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
}
