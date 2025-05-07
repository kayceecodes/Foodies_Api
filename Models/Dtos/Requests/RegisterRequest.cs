using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Requests;

public class RegisterRequest
{
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required, RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required"), RegularExpression("^(?=.*[A-Z])(?=.*[\\W_]).+$", ErrorMessage = "Password is not valid")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Street Address is required")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        public string Zipcode { get; set; }
}
