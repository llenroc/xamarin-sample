using System;
using Acr.UserDialogs;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class AddSonViewModel : BaseViewModel
    {
        public AddSonViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger) : base(userDialogs, mvxMessenger)
        {
        }
    }
}
