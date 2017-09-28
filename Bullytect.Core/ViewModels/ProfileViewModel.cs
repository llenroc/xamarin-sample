
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Messages;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using Bullytect.Core.Utils;
using Bullytect.Rest.Models.Exceptions;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {

        readonly IParentService _parentService;
        readonly IImagesService _imagesService;


        public ProfileViewModel(IParentService parentService, IUserDialogs userDialogs, 
                                IMvxMessenger mvxMessenger, IImagesService imagesService): base(userDialogs, mvxMessenger)
        {
            _parentService = parentService;
            _imagesService = imagesService;

            SaveChangesCommand = ReactiveCommand.CreateFromObservable<string, ParentEntity>((_) => _parentService.Update(SelfParent.FirstName, SelfParent.LastName, SelfParent.Birthdate, SelfParent.Email, SelfParent.Telephone));
 
            SaveChangesCommand.Subscribe(AccountUpdated);

            SaveChangesCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Profile_Saving_Changes));

			SaveChangesCommand.ThrownExceptions.Subscribe(HandleExceptions);

            LoadProfileCommand = ReactiveCommand.CreateFromObservable<string, ParentEntity>((param) => _parentService.GetProfileInformation());

            LoadProfileCommand.ToProperty(this, x => x.SelfParent, out _selfParent);

            LoadProfileCommand.IsExecuting.Subscribe((isLoading) => HandleIsExecuting(isLoading, AppResources.Profile_Loading_Data));

			LoadProfileCommand.ThrownExceptions.Subscribe(HandleExceptions);

            DeleteAccountCommand = ReactiveCommand
                .CreateFromObservable<string, string>((param) =>
                {
                    return Observable.FromAsync<bool>((_) => _userDialogs.ConfirmAsync(new ConfirmConfig()
                    {
                        Title = AppResources.Profile_Confirm_Account_Deleting

                    })).Where((confirmed) => confirmed).Do((_) => _userDialogs.ShowLoading(AppResources.Profile_Account_Deleting))
						.SelectMany((_) => _parentService.DeleteAccount())
                                     .Do((_) => _userDialogs.HideLoading());
                });

            DeleteAccountCommand.Subscribe((_) => {
                _userDialogs.ShowSuccess(AppResources.Profile_Account_Deleted);
            });

			DeleteAccountCommand.ThrownExceptions.Subscribe(HandleExceptions);


			SignOutCommand = ReactiveCommand
				.CreateFromObservable(() =>
				{
                    return Observable.FromAsync<bool>((_) => _userDialogs.ConfirmAsync(new ConfirmConfig()
                    {
                        Title = AppResources.Profile_Confirm_SignOut

                    })).Where((confirmed) => confirmed).Do((_) => _mvxMessenger.Publish(new SignOutMessage(this)));
				});


            TakePhotoCommand = CommandFactory.CreateTakePhotoCommand(_parentService, _imagesService, _userDialogs);

            TakePhotoCommand.Subscribe((image) => {
                _userDialogs.ShowSuccess(AppResources.Profile_Updating_Profile_Image_Success);
            });
            TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);

		}

        #region Properties

        protected ObservableAsPropertyHelper<ParentEntity> _selfParent;
		public ParentEntity SelfParent
		{
			get { return _selfParent.Value; }
		}

        #endregion

        #region commands


        public ReactiveCommand<string, ParentEntity> SaveChangesCommand { get; protected set; }

        public ReactiveCommand<string, string> DeleteAccountCommand { get; protected set; }

        public ReactiveCommand<string, ParentEntity> LoadProfileCommand { get; protected set; }

        public ReactiveCommand SignOutCommand { get; protected set; }

        public ReactiveCommand<string, ImageEntity> TakePhotoCommand { get; set; }


		#endregion

		void AccountUpdated(ParentEntity parent)
		{
			Debug.WriteLine(String.Format("Parent: {0}", parent.ToString()));
            var toastConfig = new ToastConfig(AppResources.Profile_Account_Updated);
			toastConfig.SetDuration(3000);
			toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
			_userDialogs.Toast(toastConfig);
		}

		protected override void HandleExceptions(Exception ex)
		{
			if (ex is UploadImageFailException)
			{
                var toastConfig = new ToastConfig(AppResources.Profile_Updating_Profile_Image_Failed);
				toastConfig.SetDuration(3000);
				toastConfig.SetBackgroundColor(System.Drawing.Color.FromArgb(12, 131, 193));
                _userDialogs.Toast(toastConfig);
			}
			else
			{
				base.HandleExceptions(ex);
			}
		}

    }
}
