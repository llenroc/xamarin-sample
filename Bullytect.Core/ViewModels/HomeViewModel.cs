
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class HomeViewModel: BaseViewModel
    {
        readonly IUserDialogs _userDialogs;
        readonly IParentService _parentService;

        public HomeViewModel(IUserDialogs userDialogs, IParentService parentService)
        {
            _userDialogs = userDialogs;
            _parentService = parentService;
        }

        ObservableAsPropertyHelper<ParentEntity> _SelfParent;
		public ParentEntity SelfParent
		{
            get { return _SelfParent.Value; }
		}

		public override void Start()
		{

            _parentService.GetProfileInformation((ex) =>
            {
                _userDialogs.ShowError(AppResources.Profile_Loading_Failed);
            }).ToProperty(this, x => x.SelfParent, out _SelfParent);
		}
	}
}
