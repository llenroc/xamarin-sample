
using System;
using System.Diagnostics;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Notification
{
    public partial class NotificationPage : MvxContentPage<NotificationViewModel>
    {
        public NotificationPage()
        {
            InitializeComponent();
        }


        private async void OnItemTapped(Object sender, ItemTappedEventArgs e)
        {
            Debug.WriteLine("Item Tapped");
        }
    }
}
