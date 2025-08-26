
namespace foodies_api.Models.Dtos.Responses;

public class LoginResponse
{
    public string Token { get; set; }
    public string Username { get; set; }
    public string FistName { get; set; }
    public string LastName { get; set; }
    public string Zipcode { get; set; }
}