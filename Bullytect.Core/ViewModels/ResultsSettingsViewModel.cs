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
			});


			RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        #region properties


        CategoryModel _allCategory;

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

        public List<PickerOptionModel> IterationsOptionsList { get; private set; } = new List<PickerOptionModel>()
            {
                new PickerOptionModel(){ Description = String.Format(AppResources.Results_Settings_Iterations_Count_Option, 10), Value = 10 },
                new PickerOptionModel(){ Description = String.Format(AppResources.Results_Settings_Iterations_Count_Option, 15), Value = 15 },
                new PickerOptionModel(){ Description = String.Format(AppResources.Results_Settings_Iterations_Count_Option, 20), Value = 20 },
                new PickerOptionModel(){ Description = String.Format(AppResources.Results_Settings_Iterations_Count_Option, 25), Value = 25 }
            };

        PickerOptionModel _iterationsOption;

        public PickerOptionModel IterationsOption
        {
            get => _iterationsOption ?? IterationsOptionsList.First((IterationOption) => IterationOption.Value.Equals(Settings.Current.IterationsCountToShow));
            set => SetProperty(ref _iterationsOption, value);
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
            Settings.Current.IterationsCountToShow = IterationsOption.Value;
            Settings.Current.FilteredSonCategories = string.Join(",", Categories?.Where(c => c.IsFiltered).Select(c => c.Identity));
        }

        #endregion
    }

}
