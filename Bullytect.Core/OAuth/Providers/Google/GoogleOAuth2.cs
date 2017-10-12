using System;
using Bullytect.Core.OAuth.Models;
using Xamarin.Forms;

namespace Bullytect.Core.OAuth.Providers.Google
{
    public partial class GoogleOAuth2 : OAuth2
    {
        partial void SetPublicNonSensitiveData();
        partial void SetPrivateSensitiveData();

        public GoogleOAuth2()
        {
            SetPublicNonSensitiveData();
            SetPrivateSensitiveData();

            return;
        }

        partial void SetPublicNonSensitiveData()
        {

            Description = "Google OAuth2";

            if (Device.RuntimePlatform == Device.Android)
                OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "828612093094-v7t95tanpgjgd68b5vnebn08d7fta524.apps.googleusercontent.com";
            else if (Device.RuntimePlatform == Device.iOS)
                OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "828612093094-9ge6q8aklgpt86id6lof9o9j3iqj8oqs.apps.googleusercontent.com";
            else
                OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "";

			OAuth2_Scope = "https://www.googleapis.com/auth/youtube";
			OAuth_UriAuthorization = new Uri("https://accounts.google.com/o/oauth2/auth");
            OAuth_UriAccessToken_UriRequestToken = new Uri("https://www.googleapis.com/oauth2/v4/token");
            OAuth_UriCallbackAKARedirect = new Uri("com.usal.bisite.bulltect:/oauth2redirect");
			AllowCancel = true;
			HowToMarkDown =
@"
    https://www.snip2code.com/Snippet/245686/Xamarin-Google-and-Facebook-authenticati
    https://console.developers.google.com/start
    https://console.developers.google.com/freetrial?authuser=1
        mcvjetko@holisticware.com
    Project ID
        xamarin-auth-component
    Project number
        1093596514437
    https://console.developers.google.com/apis/credentials?project=xamarin-auth-component&authuser=1
    
  OAuth 2.0 client IDs
        Name    
            Web client 1
        Creation date   
            Apr 3, 2015
        Type    
            Web application
        Client ID   
        1093596514437-ibfmn92v4bf27tto068heesgaohhto7n.apps.googleusercontent.com
        
        
        //  https://www.snip2code.com/Snippet/245686/Xamarin-Google-and-Facebook-authenticati
        //  https://console.developers.google.com/start
        //  https://console.developers.google.com/freetrial?authuser=1
        //      mcvjetko@holisticware.com
        //  Project ID
        //      xamarin-auth-component
        //  Project number
        //      1093596514437
        //  https://console.developers.google.com/apis/credentials?project=xamarin-auth-component&authuser=1
        //  
        //  OAuth 2.0 client IDs
        //      Name    
        //          Web client 1
        //      Creation date   
        //          Apr 3, 2015
        //      Type    
        //          Web application
        //      Client ID   
        //      1093596514437-ibfmn92v4bf27tto068heesgaohhto7n.apps.googleusercontent.com
";
			//  clientId: 123456789.apps.googleusercontent.com, 
			//  scope: https://www.googleapis.com/auth/userinfo.email, 
			//  authorizeUrl: new Uri (https://accounts.google.com/o/oauth2/auth),
			//  redirectUrl: new Uri (http://bunchy.com/oauth2callback), 
			//  getUsernameAsync: null)

			//  clientId: "123456789.apps.googleusercontent.com", 
			//  scope: "https://www.googleapis.com/auth/userinfo.email", 
			//  authorizeUrl: new Uri ("https://accounts.google.com/o/oauth2/auth"),
			//  redirectUrl: new Uri ("http://bunchy.com/oauth2callback"), 
			//  getUsernameAsync: null)


			return;
		}
    }
}
