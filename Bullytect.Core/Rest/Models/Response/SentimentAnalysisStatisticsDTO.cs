using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class SentimentAnalysisStatisticsDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("title")]
		public string Title { get; set; }

        [JsonProperty("sentiment_data")]
        public IList<SentimentDTO> SentimentData { get; set; }


        public class SentimentDTO {

			[JsonProperty("type")]
			public string Type { get; set; }

			[JsonProperty("score")]
            public float Score { get; set; }

			[JsonProperty("label")]
			public string Label { get; set; }

        }

    }
}
