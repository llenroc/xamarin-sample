using System;
namespace Bullytect.Core.ViewModels.Core.Models
{
    public class UserListModel
    {
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public string SocialMediaType { get; set; }
        public int Value { get; set; }
        public string ValueLabel { get; set; }

		public Uri ProfileImageUri
		{
			get
			{
				try
				{
					return new Uri(ProfileImage);
				}
				catch
				{

				}
				return null;
			}
		}
    }
}
