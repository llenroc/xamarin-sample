using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class ResultsSettingsViewModel : BaseViewModel
    {
        public ResultsSettingsViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {
        }

		#region properties

    		public List<PickerOptionModel> IterationsOptionsList { get; set; } = new List<PickerOptionModel>()
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

		#endregion

        void SaveChanges() {

            Settings.Current.IterationsCountToShow = IterationsOption.Value;
        }
	}

}
