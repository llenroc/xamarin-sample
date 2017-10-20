using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class CommunitiesStatisticsDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("title")]
		public string Title { get; set; }

        [JsonProperty("communities")]
        public IList<CommunityDTO> Communities { get; set; }


        public class CommunityDTO {

            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("value")]
            public int Value { get; set; }
			[JsonProperty("label")]
			public string Label { get; set; }
        }

    }
}
