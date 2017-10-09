using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class IterationDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("finish_date")]
        public DateTime FinishDate { get; set; }

        [JsonProperty("total_tasks")]
        public int TotalTasks { get; set; }

        [JsonProperty("total_failed_tasks")]
        public int TotalFailedTasks { get; set; }

		[JsonProperty("total_comments")]
		public int TotalComments { get; set; }



    }
}
