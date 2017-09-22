using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Response
{
    public class AlertDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("identity")]
		public string Identity { get; set; }

		[JsonProperty("level")]
		public string Level { get; set; }

		[JsonProperty("payload")]
		public string Payload { get; set; }

		[JsonProperty("create_at")]
		public string CreateAt { get; set; }

		[JsonProperty("son")]
		public SonDTO Son { get; set; }

		
    }
}
