using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class ImageDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("id")]
        public string Identity { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
    }
}
