using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class SaveUserSystemPreferencesDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("push_notifications_enabled")]
		public bool PushNotificationsEnabled { get; set; }
    }
}
