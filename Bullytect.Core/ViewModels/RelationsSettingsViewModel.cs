using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Utils;
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class RelationsSettingsViewModel : BaseViewModel
    {

        public RelationsSettingsViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, 
                                        AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {

            IsDirtyMonitoring = true;

        }

        #region properties

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

		#region methods



        void SaveChanges()
        {
            Settings.Current.TimeInterval = TimeIntervalOption.Value;
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
