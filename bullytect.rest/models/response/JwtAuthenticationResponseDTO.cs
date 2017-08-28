using System;
using Newtonsoft.Json;

namespace bullytect.rest.models.response
{
    public class JwtAuthenticationResponseDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("token")]
        public string Token { get; set; }
    }
}
