using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class UpdateSonDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("identity")]
		public string Identity { get; set; }

		[JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("birthdate")]
        public string Birthdate { get; set; }

        [JsonProperty("school")]
        public string School { get; set; }
    }
}
