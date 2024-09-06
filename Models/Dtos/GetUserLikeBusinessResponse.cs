using System;

namespace foodies_api.Models.Dtos;

public class GetUserLikeBusinessResponse
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public int BusinessId { get; set; }
    public string BusinessName { get; set; }
}
