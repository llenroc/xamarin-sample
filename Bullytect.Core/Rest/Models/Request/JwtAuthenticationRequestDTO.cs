using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class JwtAuthenticationRequestDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; } 
    }
}
