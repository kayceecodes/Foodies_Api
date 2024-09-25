using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Models;

[PrimaryKey(nameof(Id))]
public class User
{
    [Required, Key]
    public Guid Id { get; set; }
    [Required]
    public string FirstAndLastName { get; set; }
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }  
    [Required]
    public string Password { get; set; }
    public virtual List<UserLikeBusiness> UserLikeBusinesses { get; set; }
}