using System;
using System.Windows.Input;
using Bullytect.Core.Rest.Utils;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common
{
    public partial class ChildProfileImage : ContentView
    {

        static int PROFILE_HEIGHT_DEFAULT = 100;
        static int PROFILE_WIDTH_DEFAULT = 100;


        public static readonly BindableProperty ClickedItemCommandProperty =
            BindableProperty.Create(
                nameof(ClickedItemCommand),
                typeof(ICommand),
                typeof(ChildProfileImage));

        public static readonly BindableProperty ProfileImageProperty = BindableProperty.Create(
            nameof(ProfileImage),
            typeof(string),
            typeof(ChildProfileImage),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var Page = bindable as ChildProfileImage;
                var newProfileImage = newValue as string;
                Page.ProfileCachedImage.WidthRequest = Page.ProfileWidth;
                Page.ProfileCachedImage.HeightRequest = Page.ProfileHeight;
                Page.ProfileCachedImage.Source = !string.IsNullOrEmpty(newProfileImage) ?
                          ImageSource.FromUri(new Uri(ApiEndpoints.GET_SON_PROFILE_IMAGE.Replace(":id", newProfileImage))) :
                          ImageSource.FromFile("user_default.png");
            });

        public static readonly BindableProperty ProfileNameProperty = BindableProperty.Create(
            nameof(ProfileName),
            typeof(string),
            typeof(ChildProfileImage),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {   
                var Page = bindable as ChildProfileImage;
                var newProfileName = newValue as string;
                Page.ProfileNameLabel.Text = newProfileName;
        
            });


        public static readonly BindableProperty BadgeTextProperty = BindableProperty.Create(
            nameof(BadgeText),
            typeof(string),
            typeof(ChildProfileImage),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var Page = bindable as ChildProfileImage;
                var newBadgeText = newValue as string;
                Page.BadgeLabel.BadgeText = newBadgeText;

            });

        public static readonly BindableProperty BadgeColorProperty = BindableProperty.Create(
            nameof(BadgeColor),
            typeof(string),
            typeof(ChildProfileImage),
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var Page = bindable as ChildProfileImage;
                var newBadgeColor = newValue as string;
                Page.BadgeLabel.BadgeBackgroundColor =Color.FromHex(newBadgeColor);

            });


        public static readonly BindableProperty BadgeCommandProperty =
            BindableProperty.Create(
                nameof(BadgeCommand),
                typeof(ICommand),
                typeof(ChildProfileImage));


        public static readonly BindableProperty ProfileIdProperty = BindableProperty.Create(
            nameof(ProfileId),
            typeof(string),
            typeof(ChildProfileImage),
            defaultValue: string.Empty);

        public static readonly BindableProperty ProfileWidthProperty = BindableProperty.Create(
            nameof(ProfileWidth),
            typeof(int),
            typeof(ChildProfileImage),
            defaultValue: PROFILE_WIDTH_DEFAULT);



        public static readonly BindableProperty ProfileHeightProperty = BindableProperty.Create(
            nameof(ProfileHeight),
            typeof(int),
            typeof(ChildProfileImage),
            defaultValue: PROFILE_HEIGHT_DEFAULT);


        /*public static readonly BindableProperty ProfileBorderColorProperty = BindableProperty.Create(
            nameof(ProfileBorderColor),
            typeof(string),
            typeof(ChildProfileImage),
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var Page = bindable as ChildProfileImage;
                var newProfileBorderColor = newValue as string;
                Page.ProfileBorderColorCircle.BorderHexColor = newProfileBorderColor;
            });*/


        public ChildProfileImage()
        {
            InitializeComponent();

            var ProfiletapGestureRecognizer = new TapGestureRecognizer();
            ProfiletapGestureRecognizer.Tapped += (s, e) => {
                ClickedItemCommand?.Execute(ProfileId);
            };

            MainContainer.GestureRecognizers.Add(ProfiletapGestureRecognizer);

            var BadgeTapGestureRecognizer = new TapGestureRecognizer();
            BadgeTapGestureRecognizer.Tapped += (s, e) => {
                BadgeCommand?.Execute(false);
            };
            BadgeLabel.GestureRecognizers.Add(BadgeTapGestureRecognizer);

        }

        #region properties

        public string ProfileId
        {
            get { return (string)GetValue(ProfileIdProperty); }
            set { SetValue(ProfileIdProperty, value); }
        }

        public string ProfileImage
        {
            get { return (string)GetValue(ProfileImageProperty); }
            set { SetValue(ProfileImageProperty, value); }
        }

        public string ProfileName
        {
            get { return (string)GetValue(ProfileNameProperty); }
            set { SetValue(ProfileNameProperty, value); }

        }

        public ICommand ClickedItemCommand
        {
            get { return (ICommand)GetValue(ClickedItemCommandProperty); }
            set { SetValue(ClickedItemCommandProperty, value); }
        }

        public int ProfileWidth
        {
            get { return (int)GetValue(ProfileWidthProperty); }
            set { SetValue(ProfileWidthProperty, value); }
        }


        public int ProfileHeight
        {
            get { return (int)GetValue(ProfileHeightProperty); }
            set { SetValue(ProfileHeightProperty, value); }
        }

        /*public string ProfileBorderColor{
            get { return (string)GetValue(ProfileBorderColorProperty); }
            set { SetValue(ProfileBorderColorProperty, value); }
        }*/

        public string BadgeText
        {
            get { return (string)GetValue(BadgeTextProperty); }
            set { SetValue(BadgeTextProperty, value); }
        }

        public string BadgeColor
        {
            get { return (string)GetValue(BadgeColorProperty); }
            set { SetValue(BadgeColorProperty, value); }
        }

        public ICommand BadgeCommand
        {
            get { return (ICommand)GetValue(BadgeCommandProperty); }
            set { SetValue(BadgeCommandProperty, value); }
        }


        #endregion
    }
}
