

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


            SignupCommand = ReactiveCommand.CreateFromObservable<ParentEntity>(RegisterTask);


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

		#endregion


        public IObservable<ParentEntity> RegisterTask()
		{
            return _parentService.register(FirstName, LastName, Age, Email, PasswordClear, ConfirmPassword);

		}


		#region commands

        public ReactiveCommand SignupCommand { get; protected set; }

		#endregion
	}
}
