using System;
namespace Bullytect.Core.Models.Domain
{
    public class SocialMediaEntity
    {
		public string Identity { get; set; }

		public string AccessToken { get; set; }

		public string Type { get; set; }

		public bool InvalidToken { get; set; }

		public string Son { get; set; }

		public override string ToString()
		{
			return String.Format("Identity: {0}, AccessToken:{1}, Type:{2}, InvalidToken:{3}, Son:{4}", Identity, AccessToken, Type, InvalidToken, Son);
		}
    }
}
