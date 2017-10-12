using System;
using Bullytect.Core.OAuth.Models;

namespace Bullytect.Core.OAuth.Providers.Instagram
{
	public partial class InstagramOAuth2 : OAuth2
	{
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public InstagramOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();

			return;
		}

		partial void SetPublicNonSensitiveData()
		{
			Description = "BullTect Instagram";
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "08a463d96c5149a8beaf00c1d911fb67";
			OAuth2_Scope = "basic, comments, public_content, relationships, likes, follower_list";
			OAuth_UriAuthorization = new Uri("https://api.instagram.com/oauth/authorize/");
			OAuth_UriCallbackAKARedirect = new Uri("ig08a463d96c5149a8beaf00c1d911fb67://authorize");
			AllowCancel = true;
			HowToMarkDown =
@"
    https://instagram.com/developer/
    https://www.instagram.com/developer/clients/manage/
    Xamarin.Auth.Component
    CLIENT INFO
    CLIENT ID       
    WEBSITE URL     http://xamarin.com
    REDIRECT URI    http://xamarin.com/
    SUPPORT EMAIL   None
    Sample for Xamarin.Auth component
";

			return;
		}
	}
}
