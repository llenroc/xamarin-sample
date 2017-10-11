using Android.App;
using Android.Content;
using Android.OS;
using System;
using Bullytect.Core.OAuth.Models;

namespace Bullytect.Droid.OAuth
{
	[Activity(Label = "OAuthAuthInterceptor")]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView },
			Categories = new[]
			{
					Intent.CategoryDefault,
					Intent.CategoryBrowsable
			},
			DataSchemes = new[]
			{
                "com.usal.bisite.bulltect"
			},
			DataPaths = new[]
			{
                // Second part of the redirect url (Path)
                "/oauth2redirect"
			}
		)
	]
	public class OAuthAuthInterceptor : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Android.Net.Uri uri_android = Intent.Data;

			// Convert iOS NSUrl to C#/netxf/BCL System.Uri - common API
			Uri uri_netfx = new Uri(uri_android.ToString());

			// Send the URI to the Authenticator for continuation
			AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

			Finish();
		}
	}
}
