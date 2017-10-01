using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class APIResponse<T> where T: class
    {
        #pragma warning disable CS1701

        [JsonProperty("one")]
        public string Code { get; set; }

        [JsonProperty("response_status")]
        public string Status { get; set; }

        [JsonProperty("response_http_status")]
        public string HttpStatus { get; set; }

		[JsonProperty("response_info_url")]
		public string InfoUrl { get; set; }

        [JsonProperty("response_data")]
        public T Data { get; set;  }
    }
}
