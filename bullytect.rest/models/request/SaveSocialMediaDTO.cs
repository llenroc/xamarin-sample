using System;
using Newtonsoft.Json;

namespace bullytect.rest.models.request
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
