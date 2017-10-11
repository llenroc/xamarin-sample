using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
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
		public DateTime Birthdate { get; set; }

		[JsonProperty("age")]
		public int Age { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("telephone")]
		public string Telephone { get; set; }

		[JsonProperty("fb_id")]
		public string FbId { get; set; }

		[JsonProperty("children")]
		public long Children { get; set; }

		[JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("profile_image")]
        public string ProfileImage { get; set; }

    }
}
