﻿namespace EMStores.Web.Models.Dtos
{
    public class LoginResponseDTO
    {
        public UserDTO? User { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
