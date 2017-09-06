using System;
namespace Bullytect.Core.Models.Domain
{
    public class DeviceEntity
    {
		public string RegistrationToken { get; set; }
        public string Type { get; set; }
        public string CreateAt { get; set; }

		public override string ToString()
		{
			return String.Format("Registration Token: {0}, Type:{1}, CreateAt:{2}", RegistrationToken, Type, CreateAt);
		}
    }
}
