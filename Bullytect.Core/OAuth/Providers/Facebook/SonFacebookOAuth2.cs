﻿using System;
using Bullytect.Core.Config;
using Bullytect.Core.I18N;
using Bullytect.Core.OAuth.Models;

namespace Bullytect.Core.OAuth.Providers.Facebook
{
    public partial class SonFacebookOAuth2: OAuth2
    {
		partial void SetPublicNonSensitiveData();
		partial void SetPrivateSensitiveData();

		public SonFacebookOAuth2()
		{
			SetPublicNonSensitiveData();
			SetPrivateSensitiveData();
		}

		partial void SetPublicNonSensitiveData()
		{
			Description = "Facebook OAuth2 WWW App Type Callbackurl http[s]://[www.]xamarin.com";
            OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer = SharedConfig.FACEBOOK_CLIENT_ID;
            OAuth_SecretKey_ConsumerSecret_APISecret = SharedConfig.FACEBOOK_CLIENT_SECRET;
			OAuth2_Scope = "user_likes, user_photos, user_posts, user_relationship_details, user_relationships, user_friends";
            OAuth_UriAccessToken_UriRequestToken = new Uri("https://graph.facebook.com/oauth/access_token");
            OAuth_UriAuthorization = new Uri("https://m.facebook.com/dialog/oauth/");
            //OAuth_UriCallbackAKARedirect = new Uri("fb341732922916068://authorize");
            OAuth_UriCallbackAKARedirect = new Uri("https://www.facebook.com/connect/login_success.html");
			AllowCancel = true;

            ResetData = true;
            UsingNativeUI = false;
            Title = AppResources.Authentication_Facebook_Enable_Title;
			HowToMarkDown =
@"
    https://developers.facebook.com/apps/
        Settings 
            Display Name = Xamarin.Auth.WWW.xamarin.com
            Advanced
                https://developers.facebook.com/apps/<AppID>/settings/advanced/
                Client OAuth Login = true
                    Enables the standard OAuth client token flow. Secure your application 
                    and prevent abuse by locking down which token redirect URIs are allowed 
                    with the options below. Disable globally if not used.
                    Disable Client OAuth Login if your app does not use it. Client OAuth 
                    Login is the global on-off switch for using OAuth client token flows. 
                    If your app does not use any client OAuth flows, which include Facebook 
                    login SDKs, you should disable this flow. Note, though, that you can't 
                    request permissions for an access token if you have Client OAuth Login 
                    disabled. This setting is found in the 
                    Products > Facebook Login > Settings section.
                Web OAuth Login = true
                    Enables web based OAuth client login for building custom login flows.
                    Disable Web OAuth Flow or Specify a Redirect Whitelist. Web OAuth Login 
                    settings enables any OAuth client token flows that use the Facebook web 
                    login dialog to return tokens to your own website. This setting is in the 
                    Products > Facebook Login > Settings section. Disable this setting if you 
                    are not building a custom web login flow or using the Facebook Login SDK 
                    on the web.
                Force Web OAuth Reauthentication = false
                    When on, prompts people to enter their Facebook password in order to 
                    log in on the web.
                    When this setting is enabled you are required to specify a list of OAuth 
                    redirect URLs. Specify an exhaustive set of app URLs that are the only 
                    valid redirect URLs for your app for returning access tokens and codes 
                    from the OAuth flow.
                Embedded Browser OAuth Login = false
                    Enables browser control redirect uri for OAuth client login.
                    Disable embedded browser OAuth flow if your app does not use it. 
                    Some desktop and mobile native apps authenticate users by doing 
                    the OAuth client flow inside an embedded webview. 
                    If your app does not do this, then disable the setting in 
                    Products > Facebook Login > Settings section.
            Valid OAuth redirect URIs
                http://xamarin.com/
                https://xamarin.com/
                http://www.xamarin.com
                https://www.xamarin.com
            using URI not listed here will cause:
                Error:
                Given URL is not allowed by the Application configuration.: 
                One or more of the given URLs is not allowed by the App's settings. 
                It must match the Website URL or Canvas URL, or the domain must be a 
                subdomain of one of the App's domains.
            ";
		}
    }
}
