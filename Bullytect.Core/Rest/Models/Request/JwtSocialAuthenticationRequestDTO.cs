using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class JwtSocialAuthenticationRequestDTO
    {
        #pragma warning disable CS1701

        [JsonProperty("token")]
        public string Token { get; set; } 
    }
}
