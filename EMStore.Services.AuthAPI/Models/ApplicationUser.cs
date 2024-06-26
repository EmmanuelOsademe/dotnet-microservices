﻿using Microsoft.AspNetCore.Identity;

namespace EMStore.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
