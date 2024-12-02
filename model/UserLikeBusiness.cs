using System;
using System.Collections.Generic;

namespace foodies_api.model;

public partial class UserLikeBusiness
{
    public Guid UserId { get; set; }

    public string BusinessId { get; set; }

    public string FullName { get; set; }

    public string BusinessName { get; set; }

    public virtual Business Business { get; set; }

    public virtual User User { get; set; }
}
