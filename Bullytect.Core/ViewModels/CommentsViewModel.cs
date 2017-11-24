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
using System.Diagnostics;

namespace Bullytect.Core.ViewModels
{
    public class CommentsViewModel : BaseViewModel
    {

        readonly IParentService _parentService;

        readonly Dictionary<DimensionCategoryEnum, string> DimensionsFilter = new Dictionary<DimensionCategoryEnum, string>();

        readonly IList<SocialMediaTypeEnum> SocialMediaFilter = new List<SocialMediaTypeEnum>();


        public CommentsViewModel(IParentService parentService, IUserDialogs userDialogs,
                               IMvxMessenger mvxMessenger, AppHelper appHelper) : base(userDialogs, mvxMessenger, appHelper)
        {
            _parentService = parentService;


            RefreshCommand = ReactiveCommand
                .CreateFromObservable<Unit, IList<CommentEntity>>((param) =>
                {
                    if (PopupNavigation.PopupStack.Count > 0)
                    {
                        PopupNavigation.PopAllAsync();
                    }

                    return _parentService.GetComments(SonIdentity, AuthorIndentity, TimeIntervalOption.Value, SocialMediaFilter, DimensionsFilter);

                });

            RefreshCommand.Subscribe((CommentEntities) =>
            {
                Comments.ReplaceRange(CommentEntities);
                ResetCommonProps();
            });

            RefreshCommand.IsExecuting.Subscribe((IsLoading) => IsBusy = IsLoading);

            RefreshCommand.ThrownExceptions.Subscribe(HandleExceptions);


            AllDimensionCategory.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "IsFiltered")
                {
                    foreach (var category in DimensionCategories)
                    {
                        category.IsEnabled = !AllDimensionCategory.IsFiltered;
                        if (AllDimensionCategory.IsFiltered)
                            category.IsFiltered = true;
                    }

                    Debug.WriteLine(DimensionsFilter);
                }
            };

            foreach (DimensionCategoryModel dimension in DimensionCategories)
            {
                dimension.PropertyChanged += (sender, e) =>
                {

                    var Dimension = sender as DimensionCategoryModel;

                    if (e.PropertyName == "IsFiltered")
                    {
                        Debug.WriteLine("Dimension -> " + dimension.Type + " Value -> " + dimension.IsFiltered);
                        if (Dimension.IsFiltered)
                        {
                            DimensionsFilter[dimension.Type] = "POSITIVE";
                        }
                        else
                        {
                            DimensionsFilter[dimension.Type] = "NEGATIVE";
                        }
                    }
                };

            }

            AllSocialMediaCategory.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "IsFiltered")
                {
                    foreach (var category in SocialMediaCategories)
                    {
                        category.IsEnabled = !AllSocialMediaCategory.IsFiltered;
                        if (AllSocialMediaCategory.IsFiltered)
                            category.IsFiltered = true;
                    }

                    Debug.WriteLine(SocialMediaFilter);
                }
            };

            foreach (SocialMediaCategoryModel socialMedia in SocialMediaCategories)
            {
                socialMedia.PropertyChanged += (sender, e) =>
                {

                    var SocialMedia = sender as SocialMediaCategoryModel;

                    if (e.PropertyName == "IsFiltered")
                    {
                        if (SocialMedia.IsFiltered)
                        {
                            SocialMediaFilter.Add(SocialMedia.Type);
                        }
                        else
                        {
                            SocialMediaFilter.Remove(SocialMedia.Type);
                        }
                    }
                };

            }

        }

        public void Init(string SonIdentity)
        {
            this.SonIdentity = SonIdentity;
        }


        public void Init(string SonIdentity, string AuthorIndentity)
        {
            this.SonIdentity = SonIdentity;
            this.AuthorIndentity = AuthorIndentity;
        }

        #region properties

        string _sonIdentity;

        public string SonIdentity
        {
            get => _sonIdentity;
            set => SetProperty(ref _sonIdentity, value);
        }

        string _authorIndentity;

        public string AuthorIndentity
        {
            get => _authorIndentity;
            set => SetProperty(ref _authorIndentity, value);
        }

        bool _enableDimensionFilter;

        public bool EnableDimensionFilter
        {
            get => _enableDimensionFilter;
            set => SetProperty(ref _enableDimensionFilter, value);

        }

        public ObservableRangeCollection<CommentEntity> Comments { get; } = new ObservableRangeCollection<CommentEntity>();

        CategoryModel _allDimensionCategory;

        public CategoryModel AllDimensionCategory
        {
            get => _allDimensionCategory ?? (_allDimensionCategory = new CategoryModel()
            {
                Name = AppResources.Comments_Dimensions_Filter_All,
                Description = AppResources.Comments_Dimensions_Filter_All_Description,
                IsEnabled = true,
                IsFiltered = false
            });
            set => SetProperty(ref _allDimensionCategory, value);
        }


        public List<DimensionCategoryModel> DimensionCategories { get; } = new List<DimensionCategoryModel>() {

            new DimensionCategoryModel() {
                Type = DimensionCategoryEnum.VIOLENCE,
                Name = AppResources.Comments_Dimensions_Violence,
                Description = AppResources.Comments_Dimensions_Violence_Description,
                IsEnabled = true,
                IsFiltered = false,
                Icon = ImageSource.FromFile("violencia.png")
            },

            new DimensionCategoryModel() {
                Type = DimensionCategoryEnum.DRUGS,
                Name = AppResources.Comments_Dimensions_Drugs,
                Description = AppResources.Comments_Dimensions_Drugs_Description,
                IsEnabled = true,
                IsFiltered = false,
                Icon = ImageSource.FromFile("drogas.png")
            },

            new DimensionCategoryModel() {
                Type = DimensionCategoryEnum.BULLYING,
                Name = AppResources.Comments_Dimensions_Bullying,
                Description = AppResources.Comments_Dimensions_Bullying_Description,
                IsEnabled = true,
                IsFiltered = false,
                Icon = ImageSource.FromFile("bullying.png")
            },

            new DimensionCategoryModel() {
                Type = DimensionCategoryEnum.ADULT,
                Name = AppResources.Comments_Dimensions_Sex,
                Description = AppResources.Comments_Dimensions_Sex_Description,
                IsEnabled = true,
                IsFiltered = false,
                Icon = ImageSource.FromFile("sexo.png")
            }

        };


        CategoryModel _allSocialMediaCategory;

        public CategoryModel AllSocialMediaCategory
        {
            get => _allSocialMediaCategory ?? (_allSocialMediaCategory = new CategoryModel()
            {
                Name = AppResources.Comments_SocialMedia_Filter_All,
                Description = AppResources.Comments_SocialMedia_Filter_All_Description,
                IsEnabled = true,
                IsFiltered = false
            });
            set => SetProperty(ref _allSocialMediaCategory, value);
        }

        public List<SocialMediaCategoryModel> SocialMediaCategories { get; } = new List<SocialMediaCategoryModel>() {

            new SocialMediaCategoryModel() {
                Type = SocialMediaTypeEnum.FACEBOOK,
                IsEnabled = true,
                IsFiltered = false
            },

            new SocialMediaCategoryModel() {
                Type = SocialMediaTypeEnum.INSTAGRAM,
                IsEnabled = true,
                IsFiltered = false
            },

            new SocialMediaCategoryModel() {
                Type = SocialMediaTypeEnum.YOUTUBE,
                IsEnabled = true,
                IsFiltered = false
            }

        };

        public List<PickerOptionModel> TimeIntervalsOptionsList { get; private set; } = new List<PickerOptionModel>()
        {
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 1), Value = 1 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 7), Value = 7 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 15), Value = 15 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 30), Value = 30 },
            new PickerOptionModel(){ Description = String.Format(AppResources.Settings_Statistics_General_Interval_Option, 60), Value = 60 }
        };

        PickerOptionModel _timeIntervalOption;

        public PickerOptionModel TimeIntervalOption
        {
            get => _timeIntervalOption ?? TimeIntervalsOptionsList.Last();
            set => SetProperty(ref _timeIntervalOption, value);
        }

        #endregion


        #region commands

        public ReactiveCommand<Unit, IList<CommentEntity>> RefreshCommand { get; protected set; }

        public ICommand ShowCommentDetailCommand => new MvxCommand<CommentEntity>((CommentEntity CommentEntity) => ShowViewModel<CommentDetailViewModel>(new CommentDetailViewModel.CommentParameter()
        {
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

        #region methods

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

        public void ClearDimensionFilter(){
            this.DimensionsFilter?.Clear();
        }

        public void UpdateDimensionFilter(){
            foreach (DimensionCategoryModel Dimension in DimensionCategories)
            {
    
                if (Dimension.IsFiltered)
                {
                    DimensionsFilter[Dimension.Type] = "POSITIVE";
                }
                else
                {
                    DimensionsFilter[Dimension.Type] = "NEGATIVE";
                }

            }
        }

        #endregion
    }
}
