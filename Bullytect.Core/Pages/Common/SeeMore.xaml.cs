
using System.Windows.Input;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common
{
    public partial class SeeMore : ContentView
    {

        public static readonly BindableProperty ClickedItemCommandProperty =
            BindableProperty.Create(
                nameof(ClickedItemCommand),
                typeof(ICommand),
                typeof(SeeMore));


        public SeeMore()
        {
            InitializeComponent();

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                ClickedItemCommand?.Execute(false);
            };

            MainContainer.GestureRecognizers.Add(tapGestureRecognizer);
        }




        #region properties

        public ICommand ClickedItemCommand
        {
            get { return (ICommand)GetValue(ClickedItemCommandProperty); }
            set { SetValue(ClickedItemCommandProperty, value); }
        }

        #endregion
    }
}
