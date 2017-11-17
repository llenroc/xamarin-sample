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

        static readonly bool ShowAllSocialMediaDefault = true;

        public bool ShowAllSocialMedia
        {
            get { return AppSettings.GetValueOrDefault(nameof(ShowAllSocialMedia), ShowAllSocialMediaDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue(nameof(ShowAllSocialMedia), value))
                    OnPropertyChanged();
            }
        }


        static readonly int LastAlertsCountDefault = 20;

		public int LastAlertsCount
		{
            get { return AppSettings.GetValueOrDefault(nameof(LastAlertsCount), LastAlertsCountDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(LastAlertsCount), value))
					OnPropertyChanged();
			}
		}

		static readonly int AntiquityOfAlertsDefault = 0;

		public int AntiquityOfAlerts
		{
			get { return AppSettings.GetValueOrDefault(nameof(AntiquityOfAlerts), AntiquityOfAlertsDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(AntiquityOfAlerts), value))
					OnPropertyChanged();
			}
		}



		static readonly string FilteredAlertCategoriesDefault = string.Empty;


		public string FilteredAlertCategories
		{
			get { return AppSettings.GetValueOrDefault(nameof(FilteredAlertCategories), FilteredAlertCategoriesDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(FilteredAlertCategories), value))
					OnPropertyChanged();
			}
		}

        static readonly string FilteredSocialMediaDefault = string.Empty;


        public string FilteredSocialMedia
        {
            get { return AppSettings.GetValueOrDefault(nameof(FilteredSocialMedia), FilteredSocialMediaDefault); }
            set
            {
                if (AppSettings.AddOrUpdateValue(nameof(FilteredSocialMedia), value))
                    OnPropertyChanged();
            }
        }

		static readonly bool ShowResultsForAllChildrenDefault = true;

		
		public bool ShowResultsForAllChildren
		{
			get { return AppSettings.GetValueOrDefault(nameof(ShowResultsForAllChildren), ShowResultsForAllChildrenDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(ShowResultsForAllChildren), value))
					OnPropertyChanged();
			}
		}

		static readonly string FilteredSonCategoriesDefault = string.Empty;


		public string FilteredSonCategories
		{
			get { return AppSettings.GetValueOrDefault(nameof(FilteredSonCategories), FilteredSonCategoriesDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(FilteredSonCategories), value))
					OnPropertyChanged();
			}
		}

		static readonly int SonStatisticsTimeIntervalDefault = 1;

		public int SonStatisticsTimeInterval
		{
			get { return AppSettings.GetValueOrDefault(nameof(SonStatisticsTimeInterval), SonStatisticsTimeIntervalDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(SonStatisticsTimeInterval), value))
					OnPropertyChanged();
			}
		}

		static readonly int TimeIntervalDefault = 1;

		public int TimeInterval
		{
			get { return AppSettings.GetValueOrDefault(nameof(TimeInterval), TimeIntervalDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(TimeInterval), value))
					OnPropertyChanged();
			}
		}


		public static string AccessTokenAndType
		{
			get => AccessToken == null ? null : $"Bearer {AccessToken}";
		}

        static readonly bool DeviceRegisteredDefault = false;

		public bool DeviceRegistered
		{
			get { return AppSettings.GetValueOrDefault(nameof(DeviceRegistered), DeviceRegisteredDefault); }
			set
			{
				if (AppSettings.AddOrUpdateValue(nameof(DeviceRegistered), value))
					OnPropertyChanged();
			}
		}

		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;
		void OnPropertyChanged([CallerMemberName]string name = "") =>
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		#endregion
	}
}
