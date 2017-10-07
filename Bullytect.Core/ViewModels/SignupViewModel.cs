

using System;
using System.Diagnostics;
using System.Reactive;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Helpers;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class SignupViewModel: BaseViewModel
    {

        readonly IParentService _parentService;

        public SignupViewModel(IParentService parentService, IUserDialogs userDialogs, 
                               IMvxMessenger mvxMessenger, AppHelper appHelper): base(userDialogs, mvxMessenger, appHelper)
        {
			_parentService = parentService;

            SignupCommand = ReactiveCommand
                .CreateFromObservable<Unit, ParentEntity>((_) => _parentService.Register(FirstName, LastName, Birthdate, Email, PasswordClear, ConfirmPassword, String.Concat(Prefix, Telephone)));

            SignupCommand.Subscribe(AccountCreated);


            SignupCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Signup_CreatingAccount));

			SignupCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        #region Properties

        string _firstName;

		public string FirstName
		{
			get => _firstName;
			set => SetProperty(ref _firstName, value);
		}

		string _lastName;

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

		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}

        string _passwordClear;

		public string PasswordClear
		{
			get => _passwordClear;
			set => SetProperty(ref _passwordClear, value);
		}


        string _confirmPassword;

		public string ConfirmPassword
		{
			get => _confirmPassword;
			set => SetProperty(ref _confirmPassword, value);
		}

        readonly string _prefix = "+34";

        public string Prefix
        {
            get => _prefix;
        }

		int _telephone;

		public int Telephone
		{
			get => _telephone;
			set => SetProperty(ref _telephone, value);
		}


        #endregion

		#region commands

        public ReactiveCommand<Unit, ParentEntity> SignupCommand { get; protected set; }

        public ICommand GoToLoginCommand => new MvxCommand(() => ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter() {
            ReasonForAuthentication = AuthenticationViewModel.NORMAL_AUTHENTICATION
        }));

        #endregion

        void AccountCreated(ParentEntity parent){
			Debug.WriteLine(String.Format("Parent: {0}", parent.ToString()));
			ShowViewModel<AuthenticationViewModel>(new AuthenticationViewModel.AuthenticationParameter()
			{
                ReasonForAuthentication = AuthenticationViewModel.SIGN_UP
			});
        }

	}
}
