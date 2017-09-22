

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Rest.Models.Exceptions;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class SignupViewModel: BaseViewModel
    {

        readonly IParentService _parentService;

        public SignupViewModel(IParentService parentService, IUserDialogs userDialogs, IMvxMessenger mvxMessenger): base(userDialogs, mvxMessenger)
        {
			_parentService = parentService;

            SignupCommand = ReactiveCommand.CreateFromObservable<string, ParentEntity>((param) => RegisterTask());

            SignupCommand.Subscribe(AccountCreated);


            SignupCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Signup_CreatingAccount));

			SignupCommand.ThrownExceptions.Subscribe(HandleExceptions);
        }

        #region Properties

        Dictionary<string, string> _fieldErrors = new Dictionary<string, string>();

        public Dictionary<string, string> FieldErrors
		{
			get => _fieldErrors;
			set => SetProperty(ref _fieldErrors, value);
		}

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
			get => _email;
			set => SetProperty(ref _passwordClear, value);
		}


        string _confirmPassword;

		public string ConfirmPassword
		{
			get => _email;
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


        public IObservable<ParentEntity> RegisterTask()
        {
            return _parentService.Register(FirstName, LastName, Birthdate, Email, PasswordClear, ConfirmPassword, String.Concat(Prefix, Telephone));

        }

		#region commands

        public ReactiveCommand<string, ParentEntity> SignupCommand { get; protected set; }

        public ICommand GoToLoginCommand => new MvxCommand(() => ShowViewModel<AuthenticationViewModel>());

        #endregion


        void AccountCreated(ParentEntity parent){
			Debug.WriteLine(String.Format("Parent: {0}", parent.ToString()));
			var toastConfig = new ToastConfig(AppResources.Signup_Account_Created);
			toastConfig.SetDuration(3000);
			toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
			_userDialogs.Toast(toastConfig);
			//ShowViewModel<AuthenticationViewModel>();
        }


		protected override void HandleExceptions(Exception ex)
		{

			if (ex is DataInvalidException)
			{
				var dataInvalidEx = (DataInvalidException)ex;
				FieldErrors = dataInvalidEx.FieldErrors;
			}
			else
			{
				base.HandleExceptions(ex);
			}
		}
	}
}
