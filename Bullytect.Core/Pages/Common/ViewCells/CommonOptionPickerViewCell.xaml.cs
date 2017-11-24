
using System.Collections.Generic;
using Bullytect.Core.Pages.Common.Extended;
using Bullytect.Core.ViewModels.Core.Models;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Common.ViewCells
{
    public partial class CommonOptionPickerViewCell : ExtendedViewCell
    {
        public static readonly BindableProperty IconProperty = BindableProperty.Create(
            nameof(Icon),
            typeof(string),
            typeof(CommonOptionPickerViewCell),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ViewCell = bindable as CommonOptionPickerViewCell;
                var newIcon = newValue as string;
                ViewCell.IconLabel.Text = newIcon;

            });

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(CommonOptionPickerViewCell),
            defaultValue: string.Empty,
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ViewCell = bindable as CommonOptionPickerViewCell;
                var newText = newValue as string;
                ViewCell.TextLabel.Text = newText;

            });

        public static readonly BindableProperty OptionsProperty = BindableProperty.Create(
            nameof(Options),
            typeof(List<PickerOptionModel>),
            typeof(CommonOptionPickerViewCell),
            defaultValue: new List<PickerOptionModel>(),
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ViewCell = bindable as CommonOptionPickerViewCell;
                var newOptions = newValue as List<PickerOptionModel>;
                ViewCell.OptionsPicker.ItemsSource = newOptions;

            });

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem),
            typeof(PickerOptionModel),
            typeof(CommonOptionPickerViewCell),
            propertyChanging: (bindable, oldValue, newValue) =>
            {
                var ViewCell = bindable as CommonOptionPickerViewCell;
                var newOption = newValue as PickerOptionModel;
                ViewCell.OptionsPicker.SelectedItem = newOption;

            });


        public CommonOptionPickerViewCell()
        {
            InitializeComponent();
        }


        #region properties

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public List<PickerOptionModel> Options
        {
            get { return (List<PickerOptionModel>)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        public PickerOptionModel SelectedItem
        {

            get { return (PickerOptionModel)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }

        }

        #endregion
    }
}
