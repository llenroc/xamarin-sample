using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class UpdateParentDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("first_name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		public string LastName { get; set; }

		[JsonProperty("birthdate")]
		public DateTime Birthdate { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		/*[JsonProperty("telephone")]
		public string Telephone { get; set; }*/
	}
}
