using System;
using Acr.UserDialogs;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class CommentsBySonViewModel : BaseViewModel
    {
        public CommentsBySonViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IImagesService imagesService) : base(userDialogs, mvxMessenger, imagesService)
        {
        }

		string _title = "Comments BY Son";

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}
    }
}
