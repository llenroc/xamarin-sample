
using System;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class SonProfileViewModel : BaseViewModel
    {

        public SonProfileViewModel(IUserDialogs userDialogs, 
                                   IMvxMessenger mvxMessenger, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {

			DeleteSonCommand = ReactiveCommand
                .CreateFromObservable(() => _appHelper.RequestConfirmation(AppResources.Son_Profile_Delete_Confirm)
                                      .Do((_) => Close(this)));

			DeleteSonCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Common_Loading));

			DeleteSonCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        public class SonParameter
        {
            public string Identity { get; set; }
            public string FullName { get; set; }
            public DateTime Birthdate { get; set; }
            public string School { get; set; }
            public string ProfileImage { get; set; }
        }


		string _identity;

		public string Identity
		{
			get => _identity;
			set => SetProperty(ref _identity, value);
		}

        string _fullName;

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        DateTime _birthdate;

        public DateTime Birthdate
        {

            get => _birthdate;
            set => SetProperty(ref _birthdate, value);
        }

        string _school;

        public string School
        {

            get => _school;
            set => SetProperty(ref _school, value);
        }

        string _profileImage;

        public string ProfileImage 
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        public void Init(SonParameter sonParameter)
        {
            Identity = sonParameter.Identity;
            FullName = sonParameter.FullName;
            Birthdate = sonParameter.Birthdate;
            School = sonParameter.School;
            ProfileImage = sonParameter.ProfileImage;
        }


        #region commands

            public ICommand EditSonCommand => new MvxCommand<string>((string Id) => ShowViewModel<EditSonViewModel>(new { SonIdentity = Id }));
            
            public ReactiveCommand DeleteSonCommand { get; protected set; }

		#endregion


	}
}
