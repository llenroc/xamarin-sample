using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Request
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
    }
}
