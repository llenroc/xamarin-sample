﻿using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class SaveSocialMediaDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("identity")]
		public string Identity { get; set; }
		[JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("type")] 
        public string Type;
        [JsonProperty("son")]
        public string Son;
    }
}
