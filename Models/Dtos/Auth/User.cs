using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace foodies_api.Data;

public class User
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string FirstAndLastName { get; set; }
    public string UserName { get; set; }
    [Required]
    public string Email { get; set; }  
    [Required]
    public string Password { get; set; }
}