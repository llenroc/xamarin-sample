using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class JwtAuthenticationResponseDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("token")]
        public string Token { get; set; }
    }
}
