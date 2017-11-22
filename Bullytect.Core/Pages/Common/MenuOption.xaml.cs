using System.Reactive;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common
{
    public partial class MenuOption : ContentView
    {

        public static readonly BindableProperty IconProperty = BindableProperty.Create(
            nameof(Icon),
            typeof(string),
            typeof(MenuOption),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) => {
                var Component = bindable as MenuOption;
                var newIcon = newValue as string;
                Component.IconLabel.Text = newIcon;
            });


        public static readonly BindableProperty ClickedItemCommandProperty =
            BindableProperty.Create(
                nameof(ClickedItemCommand),
                typeof(ICommand),
                typeof(MenuOption));

        public MenuOption()
        {
            InitializeComponent();

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                ClickedItemCommand?.Execute(new Unit());
            };

            MainContainer.GestureRecognizers.Add(tapGestureRecognizer);
        }

        #region properties

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public ICommand ClickedItemCommand
        {
            get { return (ICommand)GetValue(ClickedItemCommandProperty); }
            set { SetValue(ClickedItemCommandProperty, value); }
        }

        #endregion
    }
}
