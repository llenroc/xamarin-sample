using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using Bullytect.Core.Config;
using Bullytect.Core.Helpers;
using Bullytect.Core.Utils;
using Bullytect.Core.ViewModels.Core.Models;
using MvvmCross.Plugins.Messenger;
using ReactiveUI;

namespace Bullytect.Core.ViewModels
{
    public class CommentsSettingsViewModel : BaseViewModel
    {
        public CommentsSettingsViewModel(IUserDialogs userDialogs,
                                         IMvxMessenger mvxMessenger, AppHelper appHelper)
            : base(userDialogs, mvxMessenger, appHelper)
        {


        }

        #region properties

        CategoryModel _allCategory;

        [IsDirtyMonitoring]
        public CategoryModel AllCategory
        {
            get => _allCategory ?? (_allCategory = new CategoryModel()
            {
                Name = "All RRSS",
                Description = "Todas los medios sociales",
                IsEnabled = true,
                IsFiltered = Settings.Current.ShowAllSocialMedia
            });
            set => SetProperty(ref _allCategory, value);
        }

        public List<SocialMediaCategoryModel> Categories { get; } = new List<SocialMediaCategoryModel>();
            


        #endregion

        #region commands

        public ReactiveCommand SaveCommand { get; protected set; }

        #endregion
    }
}
