using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class SocialMediaLikesStatisticsDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("title")]
		public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("likes")]
        public IList<SocialMediaLikesDTO> Data { get; set; }


        public class SocialMediaLikesDTO {

			[JsonProperty("type")]
            public string Type { get; set; }

			[JsonProperty("likes")]
            public int Likes { get; set; }

			[JsonProperty("label")]
			public string Label { get; set; }

        }

    }
}
