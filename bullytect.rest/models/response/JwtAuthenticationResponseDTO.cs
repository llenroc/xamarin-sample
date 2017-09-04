using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Response
{
    public class JwtAuthenticationResponseDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("token")]
        public string Token { get; set; }
    }
}
