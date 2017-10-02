
using System.Diagnostics;
using Xamarin.Auth;
using Xamarin.Forms;
using Bullytect.Core.OAuth.Models;
using Bullytect.Core.OAuth.Services;
using System;
using System.Reactive.Linq;
using Bullytect.Core.OAuth.Exceptions;
using Android.App;

[assembly: Dependency(typeof(Bullytect.Droid.Providers.OAuth))]
namespace Bullytect.Droid.Providers
{
    public class OAuth: IOAuth
    {
        
        public IObservable<string> authenticate(OAuth2 oauth2Info)
        {

            Debug.WriteLine("Authenticate By Facebook ...");

            var activity = Forms.Context as Activity;

			var auth = new OAuth2Authenticator(
                clientId: oauth2Info.OAuth_IdApplication_IdAPI_KeyAPI_IdClient_IdCustomer,
                scope: oauth2Info.OAuth2_Scope,
                authorizeUrl: oauth2Info.OAuth_UriAuthorization,
                redirectUrl: oauth2Info.OAuth_UriCallbackAKARedirect);

            IObservable<string> observable = Observable.FromEventPattern<EventHandler<AuthenticatorCompletedEventArgs>, AuthenticatorCompletedEventArgs>(
                h => auth.Completed += h,
                h => auth.Completed -= h)
                .Select(eventPattern =>
                {
                    activity.Finish();

                    if (!eventPattern.EventArgs.IsAuthenticated)
                        Observable.Throw<FacebookAuthenticationErrorException>(new FacebookAuthenticationErrorException());

                    var accessToken = eventPattern?.EventArgs?.Account?.Properties["access_token"]?.ToString();

                    return accessToken;

                });
			

            global::Android.Content.Intent ui_object = auth.GetUI(activity);

            activity.StartActivity(ui_object);

            return observable;


		}
    }
}
