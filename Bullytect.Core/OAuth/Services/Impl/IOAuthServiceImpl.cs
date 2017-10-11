using System;
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
				scope: oauth2Info.OAuth2_Scope,
				authorizeUrl: oauth2Info.OAuth_UriAuthorization,
				redirectUrl: oauth2Info.OAuth_UriCallbackAKARedirect,
                isUsingNativeUI: true);

			IObservable<string> observable = Observable.FromEventPattern<EventHandler<AuthenticatorCompletedEventArgs>, AuthenticatorCompletedEventArgs>(
				h => auth.Completed += h,
				h => auth.Completed -= h)
				.Select(eventPattern =>
				{

                    if (!eventPattern.EventArgs.IsAuthenticated)
						Observable.Throw<OAuthAuthenticationErrorException>(new OAuthAuthenticationErrorException());

					var accessToken = eventPattern?.EventArgs?.Account?.Properties["access_token"]?.ToString();

					return accessToken;

				});

            AuthenticationState.Authenticator = auth;

			var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login(auth);

			return observable;
        }

        public async Task<string> AuthenticateAsync(OAuth2 oauth2Info)
        {
            var auth = new OAuth2Authenticator(
                         clientId: oauth2Info.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                         scope: oauth2Info.OAuth2_Scope,
                         authorizeUrl: oauth2Info.OAuth_UriAuthorization,
                         redirectUrl: oauth2Info.OAuth_UriCallbackAKARedirect);

            var tcs1 = new TaskCompletionSource<AuthenticatorCompletedEventArgs>();
            EventHandler<AuthenticatorCompletedEventArgs> d1 =
                (o, e) =>
                {
                    try
                    {
                        tcs1.TrySetResult(e);
                    }
                    catch (Exception ex)
                    {
                        tcs1.TrySetResult(new AuthenticatorCompletedEventArgs(null));
                    }
                };

            try
            {
                auth.Completed += d1;
                var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                presenter.Login(auth);
                var result = await tcs1.Task;
                return result.IsAuthenticated ?
                             result?.Account?.Properties["access_token"]?.ToString() : null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                auth.Completed -= d1;
            }

        }
    }
}
