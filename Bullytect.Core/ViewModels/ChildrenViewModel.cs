
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;
using System.Reactive;
using Bullytect.Core.Helpers;
using Bullytect.Core.Exceptions;
using Bullytect.Core.I18N;
using System.Reactive.Linq;
using System.Linq;

namespace Bullytect.Core.ViewModels
{
    public class ChildrenViewModel : BaseViewModel
    {

        readonly IParentService _parentsService;

        public ChildrenViewModel(IUserDialogs userDialogs, IMvxMessenger mvxMessenger, 
                                 IParentService parentsService, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {

            _parentsService = parentsService;


            LoadChildrenCommand = ReactiveCommand.CreateFromObservable<Unit, IList<SonEntity>>((param) => _parentsService.GetChildren());

            LoadChildrenCommand.Subscribe((children) =>
            {
                Debug.WriteLine("Children Count " + children?.Count);
                Children = children;
                DataFound = children?.Count > 0;
            });

            LoadChildrenCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

            LoadChildrenCommand.ThrownExceptions.Subscribe(HandleExceptions);


            TakePhotoCommand = ReactiveCommand.CreateFromObservable<string, ImageEntity>((SonIdentity) =>
            {
                return _appHelper.PickPhotoStream()
                    .Select(MediaFile => MediaFile.GetStream())
                    .Do((_) => _userDialogs.ShowLoading(AppResources.Profile_Updating_Profile_Image))
                                 .SelectMany((FileStream) => _parentsService.UploadSonProfileImage(SonIdentity, FileStream))
                                 .Do((Image) =>
                                 {
                                     var SonEntity = Children.FirstOrDefault((Son) => Son.Identity.Equals(SonIdentity));
                                     if(SonEntity != null)
                                        SonEntity.Identity = Image.Identity;
                                  })
                                 .Do((_) => _userDialogs.HideLoading());
            });

            TakePhotoCommand.Subscribe((image) =>
            {
                _userDialogs.ShowSuccess(AppResources.Profile_Updating_Profile_Image_Success);
            });

            TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }


        #region properties

        IList<SonEntity> _children = new List<SonEntity>();

        public IList<SonEntity> Children
        {
            get => _children;
            set => SetProperty(ref _children, value);
        }

        #endregion

        #region commands

        public ReactiveCommand<Unit, IList<SonEntity>> LoadChildrenCommand { get; protected set; }


        public ICommand GoToAddSonCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<EditSonViewModel>());
            }
        }


		public ICommand ShowSonProfileCommand
		{
			get
			{
                return new MvxCommand<SonEntity>((SonEntity) => ShowViewModel<SonProfileViewModel>(new { SonEntity.Identity }));
			}
		}

		public ICommand ShowSonStatisticsCommand
        {
            get
            {
                return new MvxCommand<SonEntity>((SonEntity) => ShowViewModel<SonStatisticsViewModel>(new SonStatisticsViewModel.SonStatisticsParameter(){
                    Identity = SonEntity.Identity,
                    FullName = SonEntity.FullName
                }));
            }
        }


        public ICommand EditSonCommand => new MvxCommand<string>((string Id) => ShowViewModel<EditSonViewModel>(new { SonIdentity = Id }));

        public ICommand GoToRelations => new MvxCommand<string>((string Id) => ShowViewModel<RelationsViewModel>(new { SonIdentity = Id }));

        public ICommand GoToAlerts {

            get {

                return new MvxCommand<SonEntity>((SonEntity) => ShowViewModel<AlertsViewModel>(new { Identity = SonEntity.Identity }));
            }
        }

        public ReactiveCommand<string, ImageEntity> TakePhotoCommand { get; set; }

        #endregion


        protected override void HandleExceptions(Exception ex)
        {
		    if (ex is NoChildrenFoundException)
			{
                DataFound = false;
            }
            else if (ex is UploadImageFailException)
            {
                _appHelper.ShowAlert(AppResources.Profile_Updating_Profile_Image_Failed);

            }
            else if (ex is UploadFileIsTooLargeException)
            {

                _appHelper.ShowAlert(((UploadFileIsTooLargeException)ex).Response.Data);
            }
            else if (ex is CanNotTakePhotoFromCameraException)
            {
                _appHelper.Toast(AppResources.Profile_Can_Not_Take_Photo_From_Camera, System.Drawing.Color.FromArgb(12, 131, 193));
            }
			else
			{
				base.HandleExceptions(ex);
			}
		}
    }
}
