using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using AutoMapper;
using Bullytect.Core.Config;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Core.Utils;
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class ResultsSettingsViewModel : BaseViewModel
    {
        readonly IParentService _parentService;

        public ResultsSettingsViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, 
                                        AppHelper appHelper, IParentService parentService) : base(userDialogs, mvxMessenger, appHelper)
        {

            _parentService = parentService;

            AllCategory.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "IsFiltered")
                    EnableAllCategories(AllCategory.IsFiltered);
            };

            RefreshCommand = ReactiveCommand.CreateFromObservable<Unit, IList<SonCategoryModel>>((_) => 
                    _parentService.GetChildren().Select((SonEntities) => Mapper.Map<IList<SonEntity>, IList<SonCategoryModel>>(SonEntities)));

			RefreshCommand.Subscribe((SonCategoryModels) =>
			{
                Categories.Clear();
                Categories.AddRange(SonCategoryModels);
                SaveChanges();
				OnSonCategoriesLoaded(Categories);
                IsDirtyMonitoring = true;
			});


			RefreshCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        #region properties


        CategoryModel _allCategory;

        [IsDirtyMonitoring]
        public CategoryModel AllCategory {
			get => _allCategory ?? ( _allCategory = new CategoryModel()
			{
				Name = "All Children",
				Description = "All Children",
				IsEnabled = true,
				IsFiltered = Settings.Current.ShowResultsForAllChildren
            });
            set => SetProperty(ref _allCategory, value);
        }

        public List<SonCategoryModel> Categories { get; } = new List<SonCategoryModel>();

		public List<PickerOptionModel> TimeIntervalsOptionsList { get; private set; } = new List<PickerOptionModel>()
		{
			new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 1), Value = 1 },
			new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 7), Value = 7 },
			new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 15), Value = 15 },
			new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 30), Value = 30 }
		};

		PickerOptionModel _timeIntervalOption;

        [IsDirtyMonitoring]
		public PickerOptionModel TimeIntervalOption
		{
			get => _timeIntervalOption ?? TimeIntervalsOptionsList.First((TimeIntervalOption) => TimeIntervalOption.Value.Equals(Settings.Current.TimeInterval));
			set => SetProperty(ref _timeIntervalOption, value);
		}


		#endregion

		#region delegates

		public delegate void SonCategoriesLoadedEvent(object sender, List<SonCategoryModel> Categories);
		public event SonCategoriesLoadedEvent SonCategoriesLoaded;

		protected virtual void OnSonCategoriesLoaded(List<SonCategoryModel> Categories)
		{
			SonCategoriesLoaded?.Invoke(this, Categories);
		}

		#endregion

		#region commands 

		public ICommand SaveCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    SaveChanges();
                    _userDialogs.ShowSuccess(AppResources.Settings_Changes_Saved);
                    Close(this);
                });
            }
        }

        public ReactiveCommand<Unit, IList<SonCategoryModel>> RefreshCommand { get; protected set; }

		#endregion

		#region methods

		private void EnableAllCategories(bool enableAll)
		{
			foreach (var category in Categories)
			{
                category.IsEnabled = !enableAll;
                category.IsFiltered = enableAll || Settings.Current.FilteredSonCategories.Contains(category.Identity);
			}
		}

        void SaveChanges()
        {
            Settings.Current.ShowResultsForAllChildren = AllCategory.IsFiltered;
            Settings.Current.TimeInterval = TimeIntervalOption.Value;
            Settings.Current.FilteredSonCategories = string.Join(",", Categories?.Where(c => c.IsFiltered).Select(c => c.Identity));
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
    }

}
