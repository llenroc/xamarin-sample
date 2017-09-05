using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Bullytect.Core.Config
{
    public static class Settings
    {
		static ISettings AppSettings
		{
			get => CrossSettings.Current;
		}


        const string ACCESS_TOKEN_KEY = "ACCESS_TOKEN";

		public static string AccessToken
		{
			get => AppSettings.GetValueOrDefault(ACCESS_TOKEN_KEY, null);
			set => AppSettings.AddOrUpdateValue(ACCESS_TOKEN_KEY, value);
		}

		public static string AccessTokenAndType
		{
			get => AccessToken == null ? null : $"Bearer {AccessToken}";
		}
    }
}
