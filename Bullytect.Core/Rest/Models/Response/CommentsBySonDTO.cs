using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class CommentsBySonDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("total")]
		public string Fullname { get; set; }

        [JsonProperty("value")]
        public long Comments { get; set; }
	}
}
