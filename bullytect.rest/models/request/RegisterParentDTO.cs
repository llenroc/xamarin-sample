using System;
using Newtonsoft.Json;

namespace bullytect.rest.models.request
{
    public class RegisterParentDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password_clear")]
        public string PasswordClear { get; set; }

        [JsonProperty("confirm_password")]
        public string ConfirmPassword { get; set; }
    }
}
