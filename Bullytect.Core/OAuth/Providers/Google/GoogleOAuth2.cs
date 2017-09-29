﻿using System;
using Bullytect.Core.OAuth.Models;

namespace Bullytect.Core.OAuth.Providers.Google
{
    public partial class GoogleOAuth2: OAuth2
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
			OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = "828612093094-6mqb97f44nkl9ann8rtdm2dnmeh28apk.apps.googleusercontent.com";
			OAuth2_Scope = "https://www.googleapis.com/auth/youtube";
			OAuth_UriAuthorization = new Uri("https://accounts.google.com/o/oauth2/auth");
			OAuth_UriCallbackAKARedirect = new Uri("http://xamarin.com");
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
