using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class AlertDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("identity")]
		public string Identity { get; set; }

		[JsonProperty("level")]
		public string Level { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("payload")]
		public string Payload { get; set; }

		[JsonProperty("create_at")]
		public DateTime CreateAt { get; set; }

		[JsonProperty("son")]
		public SonDTO Son { get; set; }

		
    }
}
