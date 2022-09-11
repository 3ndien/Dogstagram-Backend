﻿namespace Dogstagram.WebApi.Features.Identity.Models
{
    public class LoginResponseModel
    {
        public ICollection<string> Roles { get; set; } = new HashSet<string>();

        public string? ImageUrl { get; set; }

        public string? Token { get; set; }
    }
}
