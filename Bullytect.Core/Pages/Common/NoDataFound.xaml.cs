
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common
{
    public partial class NoDataFound : ContentView
    {
        public NoDataFound()
        {
            InitializeComponent();
        }

		private static readonly BindableProperty MainTextProperty =
			BindableProperty.Create<NoDataFound, string>(w => w.MainText, default(string));

		private static readonly BindableProperty DetailTextProperty =
			BindableProperty.Create<NoDataFound, string>(w => w.DetailText, default(string));

		public string MainText
		{
			get { return (string)GetValue(MainTextProperty); }
			set { SetValue(MainTextProperty, value); }
		}


        public string DetailText
        {
			get { return (string)GetValue(DetailTextProperty); }
			set { SetValue(DetailTextProperty, value); }
        }

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == MainTextProperty.PropertyName)
			{
				MainTextLabel.Text = MainText;
			}

			if (propertyName == DetailTextProperty.PropertyName)
			{
				DetailTextLabel.Text = DetailText;
			}
		}
    }
}
