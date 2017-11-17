using System;
using System.Collections.Generic;
using System.Windows.Input;
using Acr.UserDialogs;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Bullytect.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;
using Bullytect.Core.Rest.Models.Exceptions;
using System.Reactive;
using System.Reactive.Linq;
using Bullytect.Core.Helpers;
using MvvmHelpers;
using Bullytect.Core.ViewModels.Core.Models;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Bullytect.Core.Pages.Comments.Popup;
using System.Linq;

namespace Bullytect.Core.ViewModels
{
    public class CommentsViewModel : BaseViewModel
    {

        readonly IParentService _parentService;

        public CommentsViewModel(IParentService parentService, IUserDialogs userDialogs, 
                               IMvxMessenger mvxMessenger, AppHelper appHelper): base(userDialogs, mvxMessenger, appHelper)
        {
            _parentService = parentService;

		

            RefreshCommand = ReactiveCommand
                .CreateFromObservable<Unit, IList<CommentEntity>>((param) => _parentService.GetCommentsBySon(SonIdentity));

            RefreshCommand.Subscribe((CommentEntities) => {
                Comments.ReplaceRange(CommentEntities);
                IsTimeout = false;
            });

            RefreshCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

			RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);

        }

        public void Init(string Identity) {
            SonIdentity = Identity;
        }

        #region properties

        string _sonIdentity;

        public string SonIdentity
        {
			get => _sonIdentity;
			set => SetProperty(ref _sonIdentity, value);
        }

        public ObservableRangeCollection<CommentEntity> Comments { get; } = new ObservableRangeCollection<CommentEntity>();


        CategoryModel _allCategory;

        public CategoryModel AllCategory
        {
            get => _allCategory ?? (_allCategory = new CategoryModel()
            {
                Name = AppResources.Comments_Dimensions_Filter_All,
                Description = AppResources.Comments_Dimensions_Filter_All_Description,
                IsEnabled = true,
                IsFiltered = false
            });
            set => SetProperty(ref _allCategory, value);
        }
        public List<DimensionCategoryModel> Categories { get; } = new List<DimensionCategoryModel>() {

            new DimensionCategoryModel() {
                Name = AppResources.Comments_Dimensions_Violence,
                Description = AppResources.Comments_Dimensions_Violence_Description,
                Icon = ImageSource.FromFile("violencia.png")
            },

            new DimensionCategoryModel() {
                Name = AppResources.Comments_Dimensions_Drugs,
                Description = AppResources.Comments_Dimensions_Drugs_Description,
                Icon = ImageSource.FromFile("drogas.png")
            },

            new DimensionCategoryModel() {
                Name = AppResources.Comments_Dimensions_Bullying,
                Description = AppResources.Comments_Dimensions_Bullying_Description,
                Icon = ImageSource.FromFile("bullying.png")
            },

            new DimensionCategoryModel() {
                Name = AppResources.Comments_Dimensions_Sex,
                Description = AppResources.Comments_Dimensions_Sex_Description,
                Icon = ImageSource.FromFile("sexo.png")
            }

        };


        public List<PickerOptionModel> TimeIntervalsOptionsList { get; private set; } = new List<PickerOptionModel>()
        {
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 1), Value = 1 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 7), Value = 7 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 15), Value = 15 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 30), Value = 30 }
        };

        PickerOptionModel _timeIntervalOption;

        public PickerOptionModel TimeIntervalOption
        {
            get => _timeIntervalOption ?? TimeIntervalsOptionsList.First();
            set => SetProperty(ref _timeIntervalOption, value);
        }

        #endregion


        #region commands

        public ReactiveCommand<Unit, IList<CommentEntity>> RefreshCommand { get; protected set; }

        public ICommand ShowCommentDetailCommand => new MvxCommand<CommentEntity>((CommentEntity CommentEntity) => ShowViewModel<CommentDetailViewModel>(new CommentDetailViewModel.CommentParameter(){
            Message = CommentEntity.Message,
            Likes = CommentEntity.Likes,
            SocialMedia = CommentEntity.SocialMedia,
            CreatedTime = CommentEntity.CreatedTime,
            ExtractedAt = CommentEntity.ExtractedAt,
            ExtractedAtSince = CommentEntity.ExtractedAtSince,
            AuthorName = CommentEntity.AuthorName,
            AuthorPhoto = CommentEntity.AuthorPhoto,
            Sentiment = CommentEntity.Sentiment,
            Violence = CommentEntity.Violence,
            Drugs = CommentEntity.Drugs,
            Bullying = CommentEntity.Bullying,
            Adult = CommentEntity.Adult
        }));


        public ICommand OpenFilterDimensionsCommand
                        => new MvxCommand(async () =>
                        {
                            if (PopupNavigation.PopupStack.Count > 0)
                            {
                                await PopupNavigation.PopAllAsync();
                            }
                            await PopupNavigation.PushAsync(new DimensionsFilterPopup(this));
                        });


		#endregion

		protected override void HandleExceptions(Exception ex)
		{

            if (ex is NoCommentsBySonFoundException)
			{
				DataFound = false;
			}
			else
			{
				base.HandleExceptions(ex);
			}
		}
	}
}
