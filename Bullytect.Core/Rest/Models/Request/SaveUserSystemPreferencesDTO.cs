using System;
using Bullytect.Core.ViewModels.Core.Models;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class SaveUserSystemPreferencesDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("push_notifications_enabled")]
		public bool PushNotificationsEnabled { get; set; }

		[JsonProperty("remove_alerts_every")]
        public string RemoveAlertsEvery { get; set; }
    }
}
