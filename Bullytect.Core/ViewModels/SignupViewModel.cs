

using System;
using System.Diagnostics;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Messages;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.Validation;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class SignupViewModel: BaseViewModel
    {

        readonly IParentService _parentService;
		readonly IValidator _validator;
        readonly IUserDialogs _userDialogs;
        readonly IMvxMessenger _mvxMessenger;

        public SignupViewModel(IValidator validator, IParentService parentService, IUserDialogs userDialogs, IMvxMessenger mvxMessenger)
        {
			_validator = validator;
			_parentService = parentService;
			_userDialogs = userDialogs;
            _mvxMessenger = mvxMessenger;

            SignupCommand = ReactiveCommand.CreateFromObservable<string, ParentEntity>((param) => RegisterTask());

            SignupCommand.Subscribe(AccountCreated);


			SignupCommand.IsExecuting.Subscribe((isLoading) => {
				if (isLoading)
				{
                    _userDialogs.ShowLoading(AppResources.Signup_CreatingAccount, MaskType.Black);
				}
				else
				{
					_userDialogs.HideLoading();
				}
			});

			SignupCommand.ThrownExceptions.Subscribe((ex) =>
			{
				Debug.WriteLine(String.Format("Exception: {0}", ex.ToString()));
				_mvxMessenger.Publish(new ExceptionOcurredMessage(this, ex));
			});
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

		private DateTime _birthdate;

		public DateTime Birthdate
		{
			get { return _birthdate; }
			set { SetProperty(ref _birthdate, value); }
		}

		string _email;

		[Required("{0} is required")]
		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}

        string _passwordClear;

		[Required("{0} is required")]
		public string PasswordClear
		{
			get => _email;
			set => SetProperty(ref _passwordClear, value);
		}


		string _confirmPassword;

		[Required("{0} is required")]
		public string ConfirmPassword
		{
			get => _email;
			set => SetProperty(ref _confirmPassword, value);
		}

		string _telephone;

		[Required("{0} is required")]
		public string Telephone
		{
			get => _telephone;
			set => SetProperty(ref _telephone, value);
		}


        #endregion


        public IObservable<ParentEntity> RegisterTask()
        {
            return _parentService.register(FirstName, LastName, Birthdate, Email, PasswordClear, ConfirmPassword, Telephone);

        }

		#region commands

        public ReactiveCommand<string, ParentEntity> SignupCommand { get; protected set; }

		#endregion


        void AccountCreated(ParentEntity parent){
			Debug.WriteLine(String.Format("Parent: {0}", parent.ToString()));
			var toastConfig = new ToastConfig(AppResources.Signup_Account_Created);
			toastConfig.SetDuration(3000);
			toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
			_userDialogs.Toast(toastConfig);
			ShowViewModel<AuthenticationViewModel>();
			Close(this);
        }
	}
}
