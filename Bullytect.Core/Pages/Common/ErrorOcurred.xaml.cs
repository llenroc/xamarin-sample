
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common
{
    public partial class ErrorOcurred : ContentView
    {
        public ErrorOcurred()
        {
            InitializeComponent();
        }

		private static readonly BindableProperty MainTextProperty =
			BindableProperty.Create<NoDataFound, string>(w => w.MainText, default(string));

		public string MainText
		{
			get { return (string)GetValue(MainTextProperty); }
			set { SetValue(MainTextProperty, value); }
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == MainTextProperty.PropertyName)
			{
				MainTextLabel.Text = MainText;
			}
		}
    }
}
