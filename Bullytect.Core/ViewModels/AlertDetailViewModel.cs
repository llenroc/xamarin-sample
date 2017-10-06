
using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class AlertDetailViewModel : BaseViewModel
    {
        public AlertDetailViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IImagesService imagesService) : base(userDialogs, mvxMessenger, imagesService)
        {
        }

        public class AlertParameter
		{
			public AlertLevelEnum Level { get; set; }
			public string Payload { get; set; }
			public DateTime CreateAt { get; set; }
            public string SonFullName { get; set; }
            public string SonIdentity { get; set; }
		}


		public void Init(AlertParameter alertParameter)
		{
            Level = alertParameter.Level;
            Payload = alertParameter.Payload;
            CreateAt = alertParameter.CreateAt;
            SonFullName = alertParameter.SonFullName;
            SonIdentity = alertParameter.SonIdentity;
		}

        #region properties

        AlertLevelEnum _level;

		public AlertLevelEnum Level
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

		DateTime _createAt;

		public DateTime CreateAt
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

		string _sonIdentity;

		public string SonIdentity
		{

			get => _sonIdentity;
			set => SetProperty(ref _sonIdentity, value);
		}


        #endregion

       
    }
}
