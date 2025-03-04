using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Responses;

public class RegistrationResponse
{    
    public string Username { get; set; }
    
    public string Email { get; set; }

    public string Token { get; set; }
}
