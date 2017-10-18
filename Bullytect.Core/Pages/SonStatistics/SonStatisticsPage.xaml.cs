
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.SonStatistics
{
    public partial class SonStatisticsPage : MvxContentPage<SonStatisticsViewModel>
    {
        public SonStatisticsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
