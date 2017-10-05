

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class AlertsPageDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("alerts")]
        public IList<AlertDTO> Alerts { get; set; }

		[JsonProperty("total")]
		public int Total { get; set; }

        [JsonProperty("returned")]
        public int Returned { get; set; }

		[JsonProperty("remaining")]
		public int Remaining { get; set; }

        [JsonProperty("last_query")]
        public DateTime LastQuery { get; set; }
	}
}
