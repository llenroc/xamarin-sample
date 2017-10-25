﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Core.Utils;
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class HomeSettingsViewModel : BaseViewModel
    {

        readonly IAlertService _alertsService;
        readonly IParentService _parentService;

        public HomeSettingsViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, AppHelper appHelper,
             IAlertService alertsService, IParentService parentService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _alertsService = alertsService;
            _parentService = parentService;

            AllCategory.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "IsFiltered")
                    FilteredAllCategories(AllCategory.IsFiltered);
            };

            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, PageModel>((_) => Observable.Zip(
                _alertsService.GetAllAlertsCategories(),
                _parentService.GetPreferences(), (AlertCategories, UserPreferences) => new PageModel() {
                UserPreferences = UserPreferences,
                AlertCategories = AlertCategories
            }));

            RefreshCommand.Subscribe((PageModel) =>
            {

                Categories.Clear();
                foreach (var category in PageModel.AlertCategories.OrderBy(c => c.Level))
                {
                    category.IsFiltered = Settings.Current.ShowAllCategories || Settings.Current.FilteredAlertCategories.Contains(category.Level.ToString());
                    category.IsEnabled = !Settings.Current.ShowAllCategories;
                    Categories.Add(category);
                }

                SaveChanges();

                OnAlertsCategoriesLoaded(Categories);

                IsPushNotificationEnabled = PageModel.UserPreferences.PushNotificationsEnabled;
                IsDirtyMonitoring = true;
            });


            RefreshCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);


            SaveCommand = ReactiveCommand.CreateFromObservable<Unit, UserSystemPreferencesEntity>((param) => _parentService.SavePreferences(IsPushNotificationEnabled));

            SaveCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

            SaveCommand.ThrownExceptions.Subscribe(HandleExceptions);

            SaveCommand.Subscribe((_) =>
            {
                SaveChanges();
                _userDialogs.ShowSuccess(AppResources.Settings_Changes_Saved);
                Close(this);
            });

        }

        #region properties

        CategoryModel _allCategory;

        [IsDirtyMonitoring]
        public CategoryModel AllCategory
        {
            get => _allCategory ?? (_allCategory = new CategoryModel()
            {
                Name = "All Alerts",
                Description = "Todas las alertas de cualquier nivel",
                IsEnabled = true,
                IsFiltered = Settings.Current.ShowAllCategories
            });
            set => SetProperty(ref _allCategory, value);
        }
        public List<AlertCategoryModel> Categories { get; } = new List<AlertCategoryModel>();


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

        [IsDirtyMonitoring]
        public PickerOptionModel AlertsOption
        {
            get => _alertsOption ?? AlertsOptionsList.First((AlertOption) => AlertOption.Value.Equals(Settings.Current.LastAlertsCount));

            set => SetProperty(ref _alertsOption, value);
        }

        PickerOptionModel _antiquityOfAlertsOption;

        [IsDirtyMonitoring]
        public PickerOptionModel AntiquityOfAlertsOption
        {
            get => _antiquityOfAlertsOption ?? AntiquityOfAlertsOptionsList.First((AntiquityOfAlertsOption) => AntiquityOfAlertsOption.Value.Equals(Settings.Current.AntiquityOfAlerts));

            set => SetProperty(ref _antiquityOfAlertsOption, value);
        }

        bool _isPushNotificationEnabled;

        [IsDirtyMonitoring]
        public bool IsPushNotificationEnabled
        {

            get => _isPushNotificationEnabled;
            set => SetProperty(ref _isPushNotificationEnabled, value);

        }

        #endregion

        #region methods

        private void FilteredAllCategories(bool showAll)
        {

            foreach (var category in Categories)
            {
                category.IsEnabled = !showAll;
                category.IsFiltered = showAll || Settings.Current.FilteredAlertCategories.Contains(category.Level.ToString());
            }
        }

        void SaveChanges()
        {
            Settings.Current.ShowAllCategories = AllCategory.IsFiltered;
            Settings.Current.LastAlertsCount = AlertsOption.Value;
            Settings.Current.AntiquityOfAlerts = AntiquityOfAlertsOption.Value;
            Settings.Current.FilteredAlertCategories = string.Join(",", Categories?.Where(c => c.IsFiltered).Select(c => c.Level.ToString()));
        }

		protected override void OnBackPressed()
		{

			if (IsDirty)
			{

                _appHelper.RequestConfirmation(AppResources.Common_Cancel_Changes)
					  .Subscribe((_) => base.OnBackPressed());
			}
			else
			{

				base.OnBackPressed();
			}
		}


        #endregion

        #region delegates

        public delegate void AlertsCategoriesLoadedEvent(object sender, List<AlertCategoryModel> Categories);
        public event AlertsCategoriesLoadedEvent AlertsCategoriesLoaded;

        protected virtual void OnAlertsCategoriesLoaded(List<AlertCategoryModel> Categories)
        {
            AlertsCategoriesLoaded?.Invoke(this, Categories);
        }

        #endregion

        #region commands 

        public ReactiveCommand<Unit, UserSystemPreferencesEntity> SaveCommand { get; protected set; }

        public ReactiveCommand<Unit, PageModel> RefreshCommand { get; protected set; }


        #endregion

        #region models

        public class PageModel {
            public UserSystemPreferencesEntity UserPreferences { get; set; }
            public IList<AlertCategoryModel> AlertCategories { get; set; }
        }

        #endregion


    }
}
