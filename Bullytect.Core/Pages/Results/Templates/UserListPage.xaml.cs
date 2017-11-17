
using System.Collections.Generic;
using Bullytect.Core.I18N;
using Bullytect.Core.ViewModels.Core.Models;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Results.Templates
{
    public partial class UserListPage : ContentView
    {

		public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
			nameof(IsLoading),
			typeof(bool),
			typeof(UserListPage),
			defaultValue: true,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var page = bindable as UserListPage;
				var isLoading = (bool)newValue;
				page.LoadingIndicator.IsLoading = isLoading;
                page.UserListView.IsVisible = !isLoading;
			});

		public static readonly BindableProperty DataFoundProperty = BindableProperty.Create(
			nameof(DataFound),
			typeof(bool),
			typeof(UserListPage),
			defaultValue: true,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var page = bindable as UserListPage;
				var DataFound = (bool)newValue;
				page.NoDataFound.IsVisible = !DataFound;
                page.UserListView.IsVisible = DataFound;
			});


		public static readonly BindableProperty ErrorProperty = BindableProperty.Create(
			nameof(Error),
			typeof(bool),
			typeof(UserListPage),
			defaultValue: false,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var page = bindable as UserListPage;
				var Error = (bool)newValue;
				page.ErrorOcurred.IsVisible = Error;
				page.PageContainer.IsVisible = !Error;
			});


		public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
			nameof(ErrorText),
			typeof(string),
			typeof(UserListPage),
			defaultValue: string.Empty,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var page = bindable as UserListPage;
				var newErrorText = (string)newValue;
				page.ErrorOcurred.MainText = newErrorText;
			});

		public static readonly BindableProperty TitleProperty = BindableProperty.Create(
			nameof(Title),
			typeof(string),
			typeof(UserListPage),
			defaultValue: string.Empty,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var page = bindable as UserListPage;
				var newTitle = newValue as string;
                page.ListTitle.Text = newTitle;
			});

		public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
		   nameof(LoadingText),
		   typeof(string),
		   typeof(UserListPage),
			defaultValue: AppResources.Common_Loading,
		   propertyChanging: (bindable, oldValue, newValue) =>
		   {
			   var page = bindable as UserListPage;
			   var newLoadingText = newValue as string;
			   page.LoadingIndicator.LoadingText = newLoadingText;
		   });


		public static readonly BindableProperty SourceProperty = BindableProperty.Create(
			nameof(Source),
			typeof(IList<UserListModel>),
			typeof(UserListPage),
			defaultValue: new List<UserListModel>(),
            propertyChanging: (bindable, oldValue, newValue) => 
            {
                var page = bindable as UserListPage;
                var newSource = newValue as IList<UserListModel>;
                page.UserListView.ItemsSource = newSource;
            });

        public UserListPage()
        {
            InitializeComponent();
        }


		#region properties

		public bool IsLoading
		{
			get { return (bool)GetValue(IsLoadingProperty); }
			set { SetValue(IsLoadingProperty, value); }
		}

		public bool DataFound
		{
			get { return (bool)GetValue(DataFoundProperty); }
			set { SetValue(DataFoundProperty, value); }

		}

		public bool Error
		{
			get { return (bool)GetValue(IsLoadingProperty); }
			set { SetValue(IsLoadingProperty, value); }
		}

		public string ErrorText
		{
			get { return (string)GetValue(ErrorTextProperty); }
			set { SetValue(ErrorTextProperty, value); }
		}

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public string LoadingText
		{
			get { return (string)GetValue(LoadingTextProperty); }
			set { SetValue(LoadingTextProperty, value); }

		}

		public IList<UserListModel> Source
		{
			get { return (IList<UserListModel>)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }

		}

		#endregion
	}
}
