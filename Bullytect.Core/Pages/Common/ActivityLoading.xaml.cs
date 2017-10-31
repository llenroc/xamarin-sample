
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common
{
    public partial class ActivityLoading : ContentView
    {
        private bool _playing = false;
        private Animation _animation;

        public static readonly BindableProperty IconProperty = BindableProperty.Create(
            nameof(Icon),
            typeof(string),
            typeof(ActivityLoading),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var activityLoading = bindable as ActivityLoading;
                var newIcon = newValue as string;
                activityLoading.IconLabel.Text = newIcon;
            });

		public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(
            nameof(IsLoading),
            typeof(bool),
            typeof(ActivityLoading),
            defaultValue: false,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var activityLoading = bindable as ActivityLoading;
                var isLoading = (bool)newValue;

                activityLoading.Container.IsVisible = isLoading;

                if(isLoading) {

    					activityLoading._animation = new Animation(
    					callback: d => activityLoading.LoadingIndicator.RotationY = d,
    					start: 0,
    					end: 360,
    					easing: Easing.Linear);

    					activityLoading._playing = true;
    					activityLoading._animation.Commit(activityLoading.LoadingIndicator, "Loop", length: 1000, repeat: () => activityLoading._playing);


                } else {

    					activityLoading._playing = false;
    					activityLoading._animation = null;
                }

			});

		public static readonly BindableProperty LoadingTextProperty = BindableProperty.Create(
			nameof(LoadingText),
			typeof(string),
			typeof(ActivityLoading),
			defaultValue: string.Empty,
			propertyChanging: (bindable, oldValue, newValue) =>
			{
				var activityLoading = bindable as ActivityLoading;
				var newLoadingText = newValue as string;
				activityLoading.LoadingTextLabel.Text = newLoadingText;
			});


        public ActivityLoading()
        {
            InitializeComponent();
        }


		#region properties

		public string Icon
		{
			get => (string)GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);

		 }

        public string LoadingText
        {

			get => (string)GetValue(LoadingTextProperty);
			set => SetValue(LoadingTextProperty, value);

        }

        #endregion



    }
}
