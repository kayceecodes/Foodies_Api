using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Responses;

public class RegisterResponse
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Zipcode { get; set; }
    public string Token { get; set; }
}
