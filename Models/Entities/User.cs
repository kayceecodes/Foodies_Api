using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Models.Entities;

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

    [Required(ErrorMessage = "Street Address is required")]
    public string StreetAddress { get; set; }

    [Required(ErrorMessage = "State is required")]
    public string State { get; set; }

    [Required(ErrorMessage = "Zipcode is required")]
    public string Zipcode { get; set; }
 
    [NotMapped]
    public virtual List<UserLikeBusiness> UserLikeBusinesses { get; set; }
}