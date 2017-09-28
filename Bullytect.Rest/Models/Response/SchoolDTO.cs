using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Response
{
    public class SchoolDTO
    {

        #pragma warning disable CS1701

        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("residence")]
        public string Residence { get; set; }

		[JsonProperty("location")]
		public string Location { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("tfno")]
        public string Tfno { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
