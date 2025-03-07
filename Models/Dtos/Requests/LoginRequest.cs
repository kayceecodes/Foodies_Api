﻿using System.ComponentModel.DataAnnotations;

namespace foodies_api.Models.Dtos.Requests;

public class LoginRequest
{
    [Required(ErrorMessage = "User Name is required")]
    public string? Username { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "E-mail is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}
