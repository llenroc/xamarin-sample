﻿
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Models.Domain;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class AlertDetailViewModel : BaseViewModel
    {
        public AlertDetailViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger) : base(userDialogs, mvxMessenger)
        {
        }

        public class AlertParameter
		{
			public string Level { get; set; }
			public string Payload { get; set; }
			public string CreateAt { get; set; }
            public string SonFullName { get; set; }
		}


		public void Init(AlertParameter alertParameter)
		{
            Level = alertParameter.Level;
            Payload = alertParameter.Payload;
            CreateAt = alertParameter.CreateAt;
            SonFullName = alertParameter.SonFullName;
		}

        #region properties

        string _level;

		public string Level
		{
			get => _level;
			set => SetProperty(ref _level, value);
		}

		string _payload;

		public string Payload
		{

			get => _payload;
			set => SetProperty(ref _payload, value);
		}

		string _createAt;

		public string CreateAt
		{

			get => _createAt;
			set => SetProperty(ref _createAt, value);
		}

		string _sonFullName;

		public string SonFullName
		{

			get => _sonFullName;
			set => SetProperty(ref _sonFullName, value);
		}

        #endregion

       
    }
}
