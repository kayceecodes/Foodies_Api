using System;

namespace foodies_api.Models.Dtos;

public class UserLikeBusinessDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string BusinessId { get; set; }
    public string BusinessName { get; set; }
}
