
using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class SonProfileViewModel : BaseViewModel
    {

        public SonProfileViewModel(IUserDialogs userDialogs, 
                                   IMvxMessenger mvxMessenger, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {
        }

        public class SonParameter
        {
            public string Identity { get; set; }
            public string FullName { get; set; }
            public DateTime Birthdate { get; set; }
            public string School { get; set; }
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

        public void Init(SonParameter sonParameter)
        {
            Identity = sonParameter.Identity;
            FullName = sonParameter.FullName;
            Birthdate = sonParameter.Birthdate;
            School = sonParameter.School;
        }


        #region commands

         public ICommand EditSonCommand => new MvxCommand<string>((string Id) => ShowViewModel<EditSonViewModel>(new { Id }));

        #endregion


    }
}
