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
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class HomeSettingsViewModel : BaseViewModel
    {

        readonly IAlertService _alertsService;

        public HomeSettingsViewModel(IUserDialogs userDialogs,
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

 
		public List<PickerOptionModel> AlertsOptionsList { get; set; } = new List<PickerOptionModel>()
    	{
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 5), Value = 5 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 10), Value = 10 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 15), Value = 15 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Alerts_Last_Alerts, 20), Value = 20 }
    	};

		public List<PickerOptionModel> AntiquityOfAlertsOptionsList { get; set; } = new List<PickerOptionModel>()
		{
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Antiquity_Of_Alerts_Description, 15), Value = 15 },
			new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Antiquity_Of_Alerts_Description, 30), Value = 30 },
			new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Antiquity_Of_Alerts_Description, 45), Value = 45 },
			new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Antiquity_Of_Alerts_Description, 60), Value = 60 }
		};

        PickerOptionModel _alertsOption;

		public PickerOptionModel AlertsOption
		{
            get => _alertsOption ?? AlertsOptionsList.First((AlertOption) => AlertOption.Value.Equals(Settings.Current.LastAlertsCount));

            set => SetProperty(ref _alertsOption, value);
		}

        PickerOptionModel _antiquityOfAlertsOption;

		public PickerOptionModel AntiquityOfAlertsOption
		{
			get => _antiquityOfAlertsOption ?? AntiquityOfAlertsOptionsList.First((AntiquityOfAlertsOption) => AntiquityOfAlertsOption.Value.Equals(Settings.Current.AntiquityOfAlerts));

            set => SetProperty(ref _antiquityOfAlertsOption, value);
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
                Settings.Current.LastAlertsCount = AlertsOption.Value;
                Settings.Current.AntiquityOfAlerts = AntiquityOfAlertsOption.Value;
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


		#endregion


    }
}
