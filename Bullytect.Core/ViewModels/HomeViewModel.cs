
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;
using Bullytect.Core.Utils;
using Bullytect.Core.Exceptions;

namespace Bullytect.Core.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        
        readonly IParentService _parentService;
        readonly IImagesService _imagesService;

        public HomeViewModel(IUserDialogs userDialogs, IParentService parentService, 
            IMvxMessenger mvxMessenger, IImagesService imagesService) : base(userDialogs, mvxMessenger)
        {
            _parentService = parentService;
            _imagesService = imagesService;


            var loadProfileCommand = ReactiveCommand
                .CreateFromObservable<string, bool>((param) =>
                {
					return _parentService.GetProfileInformation().Do((parent) =>
					{
                        Debug.WriteLine("Parent Profile " + parent?.ToString());
						SelfParent = parent;
					}).Select((_) => true);
                });

            var loadChildrenCommand = ReactiveCommand.CreateFromObservable<string, bool>((param) => {
                return _parentService.GetChildren().Do((children) =>
                {
                    Debug.WriteLine("Children Count " + children?.Count);
                    Children = children;
                }).Select((_) => true);
            });


			RefreshCommand = ReactiveCommand.CreateCombined(new[] { loadProfileCommand, loadChildrenCommand });

            RefreshCommand.IsExecuting.ToProperty(this, x => x.IsBusy, out _isBusy);

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

            TakePhotoCommand = CommandFactory.CreateTakePhotoCommand(_parentService, _imagesService, _userDialogs);

            TakePhotoCommand.Subscribe((image) => {
                _userDialogs.ShowSuccess(AppResources.Profile_Updating_Profile_Image_Success);
            });
            TakePhotoCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        #region properties

        ParentEntity _selfParent = null;

        public ParentEntity SelfParent
		{
			get => _selfParent;
			set => SetProperty(ref _selfParent, value);
		}

        IList<SonEntity> _children = new List<SonEntity>();

		public IList<SonEntity> Children
		{
			get => _children;
			set => SetProperty(ref _children, value);
		}

        bool _noChildrenFound;

        public bool NoChildrenFound
        {
            get => _noChildrenFound;
            set => SetProperty(ref _noChildrenFound, value);
        }

        #endregion


        #region commands

        public ReactiveCommand RefreshCommand { get; protected set; }

        public ICommand GoToProfileCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<ProfileViewModel>());
			}
		}


		public ICommand GoToChildrenCommand
		{
			get
			{
				return new MvxCommand(() => ShowViewModel<ChildrenViewModel>());
			}
		}

        public ICommand AddSonCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<EditSonViewModel>());
            }
        }
        

        public ICommand ShowSonProfileCommand => new MvxCommand<SonEntity>((SonEntity SonEntity) => ShowViewModel<SonProfileViewModel>(new SonProfileViewModel.SonParameter(){
            FullName = SonEntity.FullName,
            Birthdate = SonEntity.Birthdate,
            School = SonEntity.School
        }));

        public ReactiveCommand<string, ImageEntity> TakePhotoCommand { get; set; }

        #endregion


        protected override void HandleExceptions(Exception ex){

			if (ex is LoadProfileFailedException)
			{
                _userDialogs.ShowError(AppResources.Home_Loading_Failed);
            } else if (ex is NoChildrenFoundException) {
                Debug.WriteLine("No Chidlren Founds");
            } else if (ex is CanNotTakePhotoFromCameraException) {
                var toastConfig = new ToastConfig(AppResources.Profile_Can_Not_Take_Photo_From_Camera);
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
