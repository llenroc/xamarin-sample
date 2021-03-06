﻿using System;
using System.Collections.Generic;
using Bullytect.Core.Rest.Utils;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common
{
    public partial class AppHeader : ContentView
    {


		public static readonly BindableProperty BackEnableProperty = BindableProperty.Create(
			nameof(BackEnable),
			typeof(bool),
			typeof(AppHeader),
            defaultValue: true,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var appHeader = bindable as AppHeader;
				var backEnable = (bool)newValue;
                appHeader.BackButton.IsVisible = backEnable;
			});


		public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
			typeof(string),
            typeof(AppHeader),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
    			{
    				var appHeader = bindable as AppHeader;
    				var newTitle = newValue as string;
                    appHeader.PageTitle.Text = newTitle;
    			});

		public static readonly BindableProperty OptionsProperty = BindableProperty.Create(
			nameof(Options),
            typeof(IList<View>),
			typeof(AppHeader),
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var appHeader = bindable as AppHeader;
				var menuOptions = newValue as IList<View>;
                appHeader.MenuOptions.Children.Clear();
                
                foreach (var menuOption in menuOptions)
                    appHeader.MenuOptions.Children.Add(menuOption);

                
			});

        public static readonly BindableProperty ProfileImageProperty = BindableProperty.Create(
            nameof(ProfileImage),
            typeof(string),
            typeof(AppHeader),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var AppHeader = bindable as AppHeader;
                var newProfileImage = newValue as string;
                AppHeader.ProfileCachedImage.IsVisible = true;
                AppHeader.ProfileCachedImage.Source = !string.IsNullOrEmpty(newProfileImage) ?
                          ImageSource.FromUri(new Uri(ApiEndpoints.GET_PARENT_PROFILE_IMAGE.Replace(":id", newProfileImage))) :
                          ImageSource.FromFile("user_default.png");
            });


        public AppHeader()
        {
            InitializeComponent();
        }

		#region properties

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public bool BackEnable
		{
			get { return (bool)GetValue(BackEnableProperty); }
			set { SetValue(BackEnableProperty, value); }
		}

        public string ProfileImage
        {
            get { return (string)GetValue(ProfileImageProperty); }
            set { SetValue(ProfileImageProperty, value); }
        }

        public IList<View> Options{
			get { return (IList<View>)GetValue(OptionsProperty); }
			set { SetValue(OptionsProperty, value); }

        }

        #endregion
    }
}
