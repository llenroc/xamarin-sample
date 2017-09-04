
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Validation;

namespace Bullytect.Core.ViewModels
{
    public class ParentProfileViewModel : BaseViewModel
    {

        readonly IParentService _parentService;
        readonly IValidator _validator;
        readonly IMvxToastService _toastService;

        public ParentProfileViewModel(IValidator validator, IParentService parentService, IMvxToastService toastService)
        {
            _validator = validator;
            _parentService = parentService;
            _toastService = toastService;
        }

        #region Properties

        string _firstName;

        [Required("{0} is required")]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        string _lastName;

        [Required("{0} is required")]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private int age;

        [Range(18, 60, "{0} must between {1} and {2}")]
        public int Age
        {
            get { return age; }
            set { SetProperty(ref age, value); }
        }

        string _email;

        [Required("{0} is required")]
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }


        #endregion

        #region commands


        public void Save()
        {

            var errors = _validator.Validate(this);
            if (!errors.IsValid)
            {
                _toastService.DisplayErrors(errors); //Display errors here.

            }
            else
            {
                using (new Busy(this))
                {
                    var parent = _parentService.update(FirstName, LastName, Age, Email);
                    _toastService.DisplayMessage("Updated");
                }
            }

        }

        public IMvxCommand SaveCommand => new MvxCommand(Save);



        #endregion


    }
}
