using System;
using System.Threading.Tasks;
using Android.Widget;
using bullytect.Droid.Providers;
using bullytect.Utils;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastNotifier))]
namespace bullytect.Droid.Providers
{
    
    public class ToastNotifier: IToastNotifier
    {
		public Task<bool> Notify(ToastNotificationType type, string title, string description, TimeSpan duration, object context = null)
		{
			var taskCompletionSource = new TaskCompletionSource<bool>();
			Toast.MakeText(Forms.Context, description, ToastLength.Short).Show();
			return taskCompletionSource.Task;
		}

		public void HideAll()
		{
		}
    }
}
