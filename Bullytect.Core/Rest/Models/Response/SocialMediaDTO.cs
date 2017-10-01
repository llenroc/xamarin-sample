using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class SocialMediaDTO
    {
        #pragma warning disable CS1701

        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("invalidToken")]
        public bool InvalidToken { get; set; }

        [JsonProperty("user")]
        public string Son { get; set;  }

    }
}
