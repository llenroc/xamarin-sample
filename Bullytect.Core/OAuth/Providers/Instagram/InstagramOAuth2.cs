using System;
using Bullytect.Core.Config;
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
            OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "63b03cfec6894833aaf0c2823b4a66fb";
			OAuth2_Scope = "basic comments public_content relationships likes follower_list";
			OAuth_UriAuthorization = new Uri("https://api.instagram.com/oauth/authorize/");
            OAuth_UriCallbackAKARedirect = new Uri(SharedConfig.REDIRECT_URL);
			AllowCancel = true;
            UsingNativeUI = false;
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
