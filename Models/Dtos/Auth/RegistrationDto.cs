using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Auth;

public class RegistrationDto
{
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Street Address is required")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        public string Zipcode { get; set; }
}
