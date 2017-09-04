using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Response
{
    public class ParentDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

		[JsonProperty("lastName")]
		public string LastName { get; set; }

		[JsonProperty("age")]
		public int Age { get; set; }

		[JsonProperty("email")]
		public int Email { get; set; }

		[JsonProperty("children")]
		public long Children { get; set; }

    }
}
