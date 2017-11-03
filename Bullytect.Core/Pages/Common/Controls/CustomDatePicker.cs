using System;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common.Controls
{
    public class CustomDatePicker : DatePicker
    {
        public static readonly new BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color), typeof(CustomDatePicker), Color.Default);

        public new Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(CustomDatePicker), true);

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }
    }
}
