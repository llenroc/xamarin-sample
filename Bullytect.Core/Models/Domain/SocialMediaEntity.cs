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
    }
}
