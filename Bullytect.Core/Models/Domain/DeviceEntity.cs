using System;


namespace Bullytect.Core.Models.Domain
{
    public class DeviceEntity
    {

        public string DeviceId { get; set; }
        public string RegistrationToken { get; set; }
        public string Type { get; set; }
        public string CreateAt { get; set; }
        public string NotificationKeyName { get; set; }
        public string NotificationKey { get; set; }

		public override string ToString()
		{
            return String.Format("DeviceId: {0}, RegistrationToken:{1}, Type:{2}, CreateAt:{3}, NotificationKeyName:{4}, NotificationKey:{5}", 
                                 DeviceId, RegistrationToken, Type, CreateAt, NotificationKeyName, NotificationKey);
		}
    }
}
