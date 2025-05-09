using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace foodies_api.Models.Entities;

[PrimaryKey(nameof(Id))]
public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }  
    public string Password { get; set; }
    public string StreetAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zipcode { get; set; }
 
    [NotMapped]
    public virtual List<UserLikeBusiness> UserLikeBusinesses { get; set; }
}