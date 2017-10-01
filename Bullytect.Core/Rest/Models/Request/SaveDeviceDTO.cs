using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class SaveDeviceDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("registration_token")]
	    public string RegistrationToken;

    }
}
