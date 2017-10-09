using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class SonDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("identity")]
        public string Identity { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
		public string LastName { get; set; }
        [JsonProperty("birthdate")]
        public DateTime Birthdate { get; set; }
        [JsonProperty("age")]
        public int Age { get; set; }
        [JsonProperty("school")]
        public SchoolNameDTO School { get; set; }
    }
}
