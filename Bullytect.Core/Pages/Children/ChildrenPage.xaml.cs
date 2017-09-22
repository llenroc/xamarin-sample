
using System;
using Bullytect.Core.Utils;
using Bullytect.Core.ViewModels;

namespace Bullytect.Core.Pages.Children
{
    public partial class ChildrenPage : MvxCarouselPage<ChildrenViewModel>
    {
        public ChildrenPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing(){
            ViewModel.LoadChildrenCommand.Subscribe((children) => {
                ItemsSource = children;
            });
        }
    }
}
