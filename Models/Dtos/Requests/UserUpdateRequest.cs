using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Requests;

public class UserUpdateRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }

    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string Password { get; set; }

    public string City { get; set; }
    public string StreetAddress { get; set; }
    public string State { get; set; }
    public string Zipcode { get; set; }
}
