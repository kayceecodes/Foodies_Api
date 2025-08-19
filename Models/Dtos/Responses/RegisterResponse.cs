using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Responses;

public class RegisterResponse
{
    public string Username { get; set; }
    public string Token { get; set; }
}
