using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Response
{
    public class DeviceDTO
    {
		#pragma warning disable CS1701

        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("registration_token")]
        public string RegistrationToken { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("create_at")]
        public string CreateAt { get; set; }

		[JsonProperty("notification_key_name")]
		public string NotificationKeyName { get; set; }

		[JsonProperty("notification_key")]
		public string NotificationKey { get; set; }
    }
}
