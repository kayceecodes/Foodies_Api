using System;
using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Entities;

public class UserLikeBusiness
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public virtual User User { get; set; }

    public string BusinessId { get; set; }
    public string BusinessName { get; set; }
    public virtual Business Business { get; set; }        
}
