
using System.Diagnostics;
using UIKit;
using Xamarin.Auth;
using Xamarin.Forms;
using Bullytect.Core.OAuth.Models;
using Bullytect.Core.OAuth.Services;
using System;
using System.Reactive.Linq;
using Bullytect.Core.OAuth.Exceptions;

[assembly: Dependency(typeof(Bullytect.iOS.Providers.OAuth))]
namespace Bullytect.iOS.Providers
{
    public class OAuth: IOAuth
    {
        
        public IObservable<string> authenticate(OAuth2 oauth2Info)
        {

            Debug.WriteLine("Authenticate By Facebook ...");

			var rootController = ((AppDelegate)(UIApplication.SharedApplication.Delegate)).Window.RootViewController;
			var navcontroller = rootController as UINavigationController;
			if (navcontroller != null)
				rootController = navcontroller.VisibleViewController;

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
                    // UI presented, so it's up to us to dimiss it on iOS
                    // dismiss ViewController with UIWebView or SFSafariViewController
                    rootController.DismissViewController(true, null);

                    if (!eventPattern.EventArgs.IsAuthenticated)
                        Observable.Throw<FacebookAuthenticationErrorException>(new FacebookAuthenticationErrorException());

                    var accessToken = eventPattern.EventArgs.Account.Properties["access_token"].ToString();

                    return accessToken;

                });
			

            UIKit.UIViewController ui_object = auth.GetUI();
			
            rootController.PresentViewController(ui_object, true, null);

            return observable;


		}
    }
}
