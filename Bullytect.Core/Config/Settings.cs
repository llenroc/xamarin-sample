using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Bullytect.Core.Config
{
    public class Settings: INotifyPropertyChanged
    {
		static ISettings AppSettings
		{
			get => CrossSettings.Current;
		}

        static Settings settings;

		/// <summary>
		/// Gets or sets the current settings. This should always be used
		/// </summary>
		/// <value>The current.</value>
		public static Settings Current
		{
			get { return settings ?? (settings = new Settings()); }
		}


        const string ACCESS_TOKEN_KEY = "ACCESS_TOKEN";
        const string FCM_TOKEN = "FCM_TOKEN";

		public static string AccessToken
		{
			get => AppSettings.GetValueOrDefault(ACCESS_TOKEN_KEY, null);
			set => AppSettings.AddOrUpdateValue(ACCESS_TOKEN_KEY, value);
		}

		const string ShowAllCategoriesKey = "SHOW_ALL_CATEGORIES";
		static readonly bool ShowAllCategoriesDefault = true;

        /// <summary>
        /// Gets or sets a value indicating whether the user wants to show all categories.
        /// </summary>
        /// <value><c>true</c> if show all categories; otherwise, <c>false</c>.</value>
        public bool ShowAllCategories
		{
			get { return AppSettings.GetValueOrDefault(ShowAllCategoriesKey, ShowAllCategoriesDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(ShowAllCategoriesKey, value))
					OnPropertyChanged();
			}
		}


        static readonly int LastAlertsCountDefault = 5;

		public int LastAlertsCount
		{
            get { return AppSettings.GetValueOrDefault(nameof(LastAlertsCount), LastAlertsCountDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(LastAlertsCount), value))
					OnPropertyChanged();
			}
		}

		static readonly int AntiquityOfAlertsDefault = 15;

		public int AntiquityOfAlerts
		{
			get { return AppSettings.GetValueOrDefault(nameof(AntiquityOfAlerts), AntiquityOfAlertsDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(AntiquityOfAlerts), value))
					OnPropertyChanged();
			}
		}



		const string FilteredCategoriesKey = "filtered_categories";
		static readonly string FilteredCategoriesDefault = string.Empty;


		public string FilteredCategories
		{
			get { return AppSettings.GetValueOrDefault(FilteredCategoriesKey, FilteredCategoriesDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(FilteredCategoriesKey, value))
					OnPropertyChanged();
			}
		}

		static readonly int IterationsCountToShowDefaultDefault = 10;

		public int IterationsCountToShow
		{
			get { return AppSettings.GetValueOrDefault(nameof(IterationsCountToShow), IterationsCountToShowDefaultDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(IterationsCountToShow), value))
					OnPropertyChanged();
			}
		}

		public static string AccessTokenAndType
		{
			get => AccessToken == null ? null : $"Bearer {AccessToken}";
		}

        public static string FcmToken
        {
            get => AppSettings.GetValueOrDefault(FCM_TOKEN, null);
            set => AppSettings.AddOrUpdateValue(FCM_TOKEN, value);
        }


		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName]string name = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		#endregion
	}
}
