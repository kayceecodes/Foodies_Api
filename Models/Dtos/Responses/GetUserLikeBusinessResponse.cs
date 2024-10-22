using System;

namespace foodies_api.Models.Dtos.Responses;

public class GetUserLikeBusinessResponse
{
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string BusinessId { get; set; }
    public string BusinessName { get; set; }
}
