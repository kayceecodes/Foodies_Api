using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Responses;

public class AuthResponse 
{
    public string Username { get; set; }
    public Boolean Success { get; set; }
    public string Message { get; set; }
    public string Zipcode { get; set; }
    public string Token { get; set; }
}
