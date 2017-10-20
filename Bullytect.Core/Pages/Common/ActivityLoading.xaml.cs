
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

        public ActivityLoading()
        {
            InitializeComponent();
        }

        public void Start()
        {
            _animation = new Animation(
                callback: d => Wrapper.RotationY = d,
                start: 0,
                end: 360,
                easing: Easing.Linear);

            _playing = true;
            _animation.Commit(Wrapper, "Loop", length: 1000, repeat: () => _playing);
        }

        public void Stop()
        {
            _playing = false;
            _animation = null;
        }

		#region properties

		public string Icon
		{
			get { return (string)GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

        #endregion



    }
}
