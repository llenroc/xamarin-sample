using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Bullytect.Core.OAuth.Exceptions;
using Bullytect.Core.OAuth.Models;
using Xamarin.Auth;

namespace Bullytect.Core.OAuth.Services.Impl
{
    public class IOAuthServiceImpl: IOAuthService
    {


        public IObservable<Dictionary<string, string>> Authenticate(OAuth2 oauth2Info)
        {

            OAuth2Authenticator auth;

            if (oauth2Info.OAuth_UriAccessToken_UriRequestToken != null)
            {
                auth = new OAuth2Authenticator(
                    clientId: oauth2Info.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                    clientSecret: string.Empty,
                    scope: oauth2Info.OAuth2_Scope,
                    authorizeUrl: oauth2Info.OAuth_UriAuthorization,
                    redirectUrl: oauth2Info.OAuth_UriCallbackAKARedirect,
                    accessTokenUrl: oauth2Info.OAuth_UriAccessToken_UriRequestToken,
                    isUsingNativeUI: oauth2Info.UsingNativeUI
                );

            }
            else
            {

                auth = new OAuth2Authenticator(
                    clientId: oauth2Info.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                    scope: oauth2Info.OAuth2_Scope,
                    authorizeUrl: oauth2Info.OAuth_UriAuthorization,
                    redirectUrl: oauth2Info.OAuth_UriCallbackAKARedirect,
                    isUsingNativeUI: oauth2Info.UsingNativeUI
                );
            }


            auth.AllowCancel = oauth2Info.AllowCancel;

            AuthenticationState.Authenticator = auth;

            IObservable<Dictionary<string, string>> observable = Observable.Merge(
                Observable.FromEventPattern<EventHandler<AuthenticatorCompletedEventArgs>, AuthenticatorCompletedEventArgs>(
                    h => auth.Completed += h,
                    h => auth.Completed -= h)
                .Select(eventPattern =>
                {

      
                    Dictionary<string, string> authDict = new Dictionary<string, string>();

                    if (eventPattern.EventArgs.IsAuthenticated)
                    {

                    if (eventPattern?.EventArgs?.Account?.Properties.ContainsKey("access_token") == true)
                        authDict.Add("access_token", eventPattern.EventArgs.Account.Properties["access_token"]);


                    if (eventPattern?.EventArgs?.Account?.Properties.ContainsKey("refresh_token") == true)
                        authDict.Add("refresh_token", eventPattern.EventArgs.Account.Properties["refresh_token"]);

                    }

                    return authDict;

                }),
				Observable.FromEventPattern<EventHandler<AuthenticatorErrorEventArgs>, AuthenticatorErrorEventArgs>(
				h => auth.Error += h,
                    h => auth.Error -= h).Select(eventPattern => new Dictionary<string, string>())
                ).Select((authDict) => {

                        if(!authDict.ContainsKey("access_token") || string.IsNullOrEmpty(authDict["access_token"]))
                            Observable.Throw<OAuthAuthenticationErrorException>(new OAuthAuthenticationErrorException());

                        AuthenticationState.Authenticator = null;

                    return authDict;
                });


			var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login(auth);

            return observable;
        }


    }
}
