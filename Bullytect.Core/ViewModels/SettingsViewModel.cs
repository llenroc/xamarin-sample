using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {

        readonly IAlertService _alertsService;

        public SettingsViewModel(IUserDialogs userDialogs,
                                 IMvxMessenger mvxMessenger, AppHelper appHelper, IAlertService alertsService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _alertsService = alertsService;


            AllCategory = new AlertCategoryEntity()
            {
                Name = "All Alerts",
                Description = "Todas las alertas de cualquier nivel",
                Level = AlertLevelEnum.ALL,
				IsEnabled = true,
                IsFiltered = Settings.Current.ShowAllCategories
            };

            AllCategory.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "IsFiltered")
                    SetShowAllCategories(AllCategory.IsFiltered);
            };

			RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, IList<AlertCategoryEntity>>((_) => _alertsService.GetAllAlertsCategories());

			RefreshCommand.Subscribe((AlertCategories) =>
			{

				Categories.Clear();
				foreach (var category in AlertCategories.OrderBy(c => c.Level))
				{
                    category.IsFiltered = Settings.Current.ShowAllCategories || Settings.Current.FilteredCategories.Contains(category.Level.ToString());
					category.IsEnabled = !Settings.Current.ShowAllCategories;
					Categories.Add(category);
				}

				Save();

				OnAlertsCategoriesLoaded(Categories);
			});


			RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        #region properties

        public AlertCategoryEntity AllCategory { get; }
        public List<AlertCategoryEntity> Categories { get; } = new List<AlertCategoryEntity>();

        public bool ShowOnlyNewAlerts
        {

            get => Settings.Current.ShowOnlyNewAlerts;
            set => Settings.Current.ShowOnlyNewAlerts = value;

        }

		public List<AlertsCountOptions> AlertsOptionsList { get; set; } = new List<AlertsCountOptions>()
    	{
            new AlertsCountOptions(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 5), Value = 5 },
            new AlertsCountOptions(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 10), Value = 10 },
            new AlertsCountOptions(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 15), Value = 15 },
            new AlertsCountOptions(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 20), Value = 20 }
    	};

		public AlertsCountOptions AlertsOption
		{
            get => AlertsOptionsList.First((AlertOption) => AlertOption.Value.Equals(Settings.Current.LastAlertsCount));

            set => Settings.Current.LastAlertsCount = value.Value;
		}

		#endregion

		#region methods

    		private void SetShowAllCategories(bool showAll)
    		{
                Settings.Current.ShowAllCategories = showAll;
    			foreach (var category in Categories)
    			{
    				category.IsEnabled = !Settings.Current.ShowAllCategories;
                    category.IsFiltered = Settings.Current.ShowAllCategories || Settings.Current.FilteredCategories.Contains(category.Level.ToString());
    			}
    		}

    	    void Save()
    		{
                Settings.Current.FilteredCategories = string.Join(",", Categories?.Where(c => c.IsFiltered).Select(c => c.Level.ToString()));
    		}


		#endregion

		#region delegates

		public delegate void AlertsCategoriesLoadedEvent(object sender, List<AlertCategoryEntity> Categories);
		public event AlertsCategoriesLoadedEvent AlertsCategoriesLoaded;

		protected virtual void OnAlertsCategoriesLoaded(List<AlertCategoryEntity> Categories)
		{
			AlertsCategoriesLoaded?.Invoke(this, Categories);
		}

		#endregion

		#region commands 

    		public ICommand SaveCommand
    		{
    			get
    			{
                    return new MvxCommand(() =>
                    {
                        Save();
                        _userDialogs.ShowSuccess(AppResources.Settings_Changes_Saved);
                        Close(this);
                    });
    			}
    		}


            public ReactiveCommand<Unit, IList<AlertCategoryEntity>> RefreshCommand{ get; protected set; }

            public ICommand ToggleShowOnlyNewAlerts
                => new MvxCommand<bool>((bool Enabled) => ShowOnlyNewAlerts = Enabled);


		#endregion

		public class AlertsCountOptions
		{
			public string Description { get; set; }
			public int Value { get; set; }
		}
    }
}
