using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class AlertsStatisticsDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("title")]
		public string Title { get; set; }

        [JsonProperty("alerts")]
        public IList<AlertLevelDTO> Data { get; set; }


        public class AlertLevelDTO {

			[JsonProperty("level")]
			public string Level { get; set; }

			[JsonProperty("total")]
            public int Total { get; set; }

			[JsonProperty("label")]
			public string Label { get; set; }

        }

    }
}
