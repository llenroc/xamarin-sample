using System;
using System.Collections.Generic;
using Bullytect.Core.I18N;
using Bullytect.Core.ViewModels;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.EditSon.Templates
{
    public partial class EditSonHeader : ContentView
    {
        public EditSonHeader()
        {
            InitializeComponent();
        }

		private static readonly BindableProperty HeaderTitleProperty =
			BindableProperty.Create<EditSonHeader, string>(w => w.HeaderTitle, default(string));


		public string HeaderTitle
		{
			get { return (string)GetValue(HeaderTitleProperty); }
			set { SetValue(HeaderTitleProperty, value); }
		}


		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == HeaderTitleProperty.PropertyName)
			{
				PageTitle.Text = HeaderTitle;
			}
		}

        public Action AddSchoolAction { get; set; } 


		void OnAddSchoolTapped(object sender, EventArgs args)
		{
            if (AddSchoolAction != null)
                AddSchoolAction();

		}
    }
}
