using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class CommentsAnalyzedStatisticsDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("title")]
		public string Title { get; set; }

        [JsonProperty("comments")]
        public IList<CommentAnalyzedDTO> Data { get; set; }


        public class CommentAnalyzedDTO {

			[JsonProperty("date")]
            public DateTime Date { get; set; }

			[JsonProperty("total")]
            public int Total { get; set; }

			[JsonProperty("label")]
			public string Label { get; set; }

        }

    }
}
