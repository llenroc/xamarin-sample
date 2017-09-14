using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Response
{
    public class ParentDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("birthdate")]
		public string birthdate { get; set; }

		[JsonProperty("age")]
		public int age { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("telephone")]
		public string Telephone { get; set; }

		[JsonProperty("fb_id")]
		public string FbId { get; set; }

		[JsonProperty("children")]
		public long Children { get; set; }

		[JsonProperty("locale")]
		public long Locale { get; set; }



    }
}
