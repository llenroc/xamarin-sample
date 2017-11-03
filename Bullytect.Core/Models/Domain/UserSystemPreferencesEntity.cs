using System;
namespace Bullytect.Core.Models.Domain
{
    public class UserSystemPreferencesEntity
    {
        #pragma warning disable CS1701

        public bool PushNotificationsEnabled { get; set; }

        public string RemoveAlertsEvery { get; set; }
	}
}
