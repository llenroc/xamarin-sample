using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class RegisterSonDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("birthdate")]
        public DateTime Birthdate { get; set; }

        [JsonProperty("school")]
        public string School { get; set; }
    }
}
