using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Request
{
    public class SaveSocialMediaDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("type")] 
        public string Type;
        [JsonProperty("son")]
        public string Son;
    }
}
