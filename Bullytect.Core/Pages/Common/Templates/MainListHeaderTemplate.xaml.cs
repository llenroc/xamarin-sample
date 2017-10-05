
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common.Templates
{
    public partial class MainListHeaderTemplate : ContentView
    {
        public MainListHeaderTemplate()
        {
            InitializeComponent();
        }

		public static BindableProperty TextProperty =
			BindableProperty.Create(
				nameof(Text),
				typeof(string),
				typeof(MainListHeaderTemplate),
				string.Empty,
				defaultBindingMode: BindingMode.OneWay,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (MainListHeaderTemplate)bindable;
					ctrl.Text = (string)newValue;
				}
			);

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set
			{
				SetValue(TextProperty, value);
				HeaderLabel.Text = value;
			}
		}

		/* ICON */

		public static BindableProperty IconTextProperty =
			BindableProperty.Create(
				nameof(IconText),
				typeof(string),
				typeof(MainListHeaderTemplate),
				string.Empty,
				defaultBindingMode: BindingMode.OneWay,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (MainListHeaderTemplate)bindable;
					ctrl.IconText = (string)newValue;
				}
			);

		public string IconText
		{
			get { return (string)GetValue(IconTextProperty); }
			set
			{
				SetValue(IconTextProperty, value);
				HeaderIcon.Text = (value);
			}
		}
    }
}
