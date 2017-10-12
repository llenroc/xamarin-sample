using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bullytect.Core.OAuth.Exceptions;
using Bullytect.Core.OAuth.Models;
using Xamarin.Auth;

namespace Bullytect.Core.OAuth.Services.Impl
{
    public class IOAuthServiceImpl: IOAuthService
    {
 

        public IObservable<string> Authenticate(OAuth2 oauth2Info)
        {
			var auth = new OAuth2Authenticator(
				clientId: oauth2Info.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                clientSecret: string.Empty,
				scope: oauth2Info.OAuth2_Scope,
				authorizeUrl: oauth2Info.OAuth_UriAuthorization,
				redirectUrl: oauth2Info.OAuth_UriCallbackAKARedirect,
				accessTokenUrl: oauth2Info.OAuth_UriAccessToken_UriRequestToken,
                isUsingNativeUI: true
            ) {
                AllowCancel = oauth2Info.AllowCancel
            };


            IObservable<string> observable = Observable.Merge(
				Observable.FromEventPattern<EventHandler<AuthenticatorCompletedEventArgs>, AuthenticatorCompletedEventArgs>(
				    h => auth.Completed += h,
                    h => auth.Completed -= h)
                .Select(eventPattern => 
                      eventPattern.EventArgs.IsAuthenticated ? 
                        eventPattern?.EventArgs?.Account?.Properties["access_token"]?.ToString() : string.Empty),
				Observable.FromEventPattern<EventHandler<AuthenticatorErrorEventArgs>, AuthenticatorErrorEventArgs>(
				h => auth.Error += h,
				h => auth.Error -= h).Select(eventPattern => string.Empty)
            ).Select((AccessToken) => {

                if(string.IsNullOrEmpty(AccessToken))
                    Observable.Throw<OAuthAuthenticationErrorException>(new OAuthAuthenticationErrorException());
                return AccessToken;
            });



            AuthenticationState.Authenticator = auth;

			var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login(auth);

            return observable;
        }


    }
}
