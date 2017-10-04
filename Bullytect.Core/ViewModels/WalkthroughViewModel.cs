
using Acr.UserDialogs;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.ViewModels
{
    public class WalkthroughViewModel : BaseViewModel
    {
        public WalkthroughViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, IImagesService imagesService) : base(userDialogs, mvxMessenger, imagesService)
        {
        }
    }
}
