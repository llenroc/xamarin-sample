﻿using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Request
{
    public class JwtFacebookAuthenticationRequestDTO
    {
        #pragma warning disable CS1701

        [JsonProperty("token")]
        public string Token { get; set; } 
    }
}