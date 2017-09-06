using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Response
{
    public class DeviceDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("registration_token")]
        public string RegistrationToken { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("create_at")]
        public string CreateAt { get; set; }
    }
}
