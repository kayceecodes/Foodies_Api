
namespace foodies_api.Models.Dtos.Responses;

public class LoginResponse
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string Zipcode { get; set; }
    public string Token { get; set; }
}