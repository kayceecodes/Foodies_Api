using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Responses;

public class RegistrationResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }  
    public string Password { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zipcode { get; set; }
    public string Token { get; set; }
}
