using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace bullytect.config
{
    public static class Settings
    {
		static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}


        const string ACCESS_TOKEN_KEY = "ACCESS_TOKEN";

		public static string AccessToken
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(ACCESS_TOKEN_KEY, null);
			}
			set
			{
				AppSettings.AddOrUpdateValue(ACCESS_TOKEN_KEY, value);
			}
		}

		public static string AccessTokenAndType
		{
			get
			{
				return AccessToken == null ? null : $"Bearer {AccessToken}";
			}
		}
    }
}
