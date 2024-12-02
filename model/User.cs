using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foodies_api.model;

public partial class User
{
    public Guid Id { get; set; }

    public string FirstAndLastName { get; set; }

    public string Username { get; set; }

    [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
    public string Email { get; set; }

    [RegularExpression("^(?=.*[A-Z])(?=.*[\\W_]).+$", ErrorMessage = "Password is not valid")]
    public string Password { get; set; }

    public virtual ICollection<UserLikeBusiness> UserLikeBusinesses { get; set; } = new List<UserLikeBusiness>();
}
