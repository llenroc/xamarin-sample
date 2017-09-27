﻿
using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class SonProfileViewModel : BaseViewModel
    {

        public SonProfileViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger) : base(userDialogs, mvxMessenger)
        {
        }

        public class SonParameter
        {
            public string Identity { get; set; }
            public string FullName { get; set; }
            public string Birthdate { get; set; }
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

        string _birthdate;

        public string Birthdate
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
            FullName = sonParameter.FullName;
            Birthdate = sonParameter.Birthdate;
            School = sonParameter.School;
        }


        #region commands

            public ICommand EditSonCommand => new MvxCommand<string>((string Id) => ShowViewModel<EditSonViewModel>(new { Id }));

        #endregion


    }
}
