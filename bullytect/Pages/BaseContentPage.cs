using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using bullytect.ViewModels;
using Bullytect.Utils.Helpers;
using Bullytect.Utils.Models;
using Xamarin.Forms;

namespace bullytect.Pages
{
	/// <summary>
	/// Each ContentPage is required to align with a corresponding ViewModel
	/// ViewModels will be the BindingContext by default
	/// </summary>
	public class BaseContentPage<T> : MainBaseContentPage where T : BaseViewModel, new()
	{
		protected T _viewModel;

		public T ViewModel
		{
			get
			{
				return _viewModel ?? (_viewModel = new T());
			}
		}

		~BaseContentPage()
		{
			_viewModel = null;
		}

		public BaseContentPage()
		{
			BindingContext = ViewModel;
		}
	}


	public class MainBaseContentPage : ContentPage
	{
        
		public Color BarTextColor
		{
			get;
			set;
		}

		public Color BarBackgroundColor
		{
			get;
			set;
		}

		public MainBaseContentPage()
		{
			BarTextColor = Color.White;
			BackgroundColor = Color.White;
		}

		public bool HasInitialized
		{
			get;
			private set;
		}

		~MainBaseContentPage()
		{
			Debug.WriteLine("Destructor called for {0} {1}".Fmt(GetType().Name, GetHashCode()));
		}

		protected virtual void OnLoaded()
		{
			TrackPage(new Dictionary<string, string>());
		}


		protected override void OnAppearing()
		{
            
            MessagingCenter.Subscribe<App, NotificationPayload>(this, EventTypeName.INCOMING_PAYLOAD_RECEIVED, (App sender, NotificationPayload payload) => {
				if (payload != null)
					OnIncomingPayload(payload);
            });

			MessagingCenter.Subscribe<App>(this, EventTypeName.USER_AUTHENTICATED, (App sender) => {
				if (App.Instance.CurrentAthlete != null)
				{
					App.Instance.ProcessPendingPayload();
				}
			});

			var nav = Parent as NavigationPage;
			if (nav != null)
			{
				nav.BarBackgroundColor = BarBackgroundColor;
				nav.BarTextColor = BarTextColor;
			}

			if (!HasInitialized)
			{
				HasInitialized = true;
				OnLoaded();
			}

			App.Instance.ProcessPendingPayload();
			base.OnAppearing();
		}


		protected override void OnDisappearing()
		{

			MessagingCenter.Unsubscribe<AuthenticationViewModel>(this, EventTypeName.USER_AUTHENTICATED);

			base.OnDisappearing();
			EvaluateNavigationStack();
		}

		

		protected virtual void UnsubscribeFromMessages()
		{
			MessagingCenter.Unsubscribe<AuthenticationViewModel>(this, EventTypeName.INCOMING_PAYLOAD_RECEIVED);
		}


		async Task EvaluateNavigationStack()
		{
			await Task.Delay(100);
			if (Navigation.NavigationStack.Count == 0)
			{
				UnsubscribeFromMessages();
			}
		}

		

		/// <summary>
		/// Wraps the ContentPage within a NavigationPage
		/// </summary>
		/// <returns>The navigation page.</returns>
		public NavigationPage WithinNavigationPage()
		{
			var nav = new ThemedNavigationPage(this);
			ApplyTheme(nav);
			return nav;
		}


		public void ApplyTheme(NavigationPage nav)
		{
			nav.BarBackgroundColor = BarBackgroundColor;
			nav.BarTextColor = BarTextColor;
		}

		public void AddDoneButton(string text = "Done", ContentPage page = null)
		{
			var btnDone = new ToolbarItem
			{
				AutomationId = "doneButton",
				Text = text,
			};

			btnDone.Clicked += async (sender, e) =>
			await Navigation.PopModalAsync();

			page = page ?? this;
			page.ToolbarItems.Add(btnDone);
		}

		protected virtual void TrackPage(Dictionary<string, string> metadata)
		{
			var identifier = GetType().Name;
			//InsightsManager.Track(identifier, metadata);
		}

		async protected virtual void OnIncomingPayload(NotificationPayload payload)
		{
			//string challengeId;

			//if(payload.Payload.TryGetValue("challengeId", out challengeId))
			//{
			//  try
			//  {
			//      var vm = new BaseViewModel();
			//      var task = AzureService.Instance.GetChallengeById(challengeId);
			//      await vm.RunSafe(task);

			//      if(task.IsCompleted && !task.IsFaulted && task.Result != null)
			//      {
			//          var details = new ChallengeDetailsPage(task.Result);
			//          details.AddDoneButton();

			//          await App.Instance.MainPage.Navigation.PushModalAsync(details.WithinNavigationPage());
			//      }
			//  }
			//  catch(Exception e)
			//  {
			//      //InsightsManager.Report(e);
			//      Console.WriteLine(e);
			//  }
			//}
		}

		#region Authentication

		public virtual async Task<bool> EnsureUserAuthenticated()
		{
			if (Navigation == null)
				throw new Exception("Navigation is null so unable to show auth form");

			var authPage = new AuthenticationPage();
			await Navigation.PushModalAsync(authPage, true);

			await Task.Delay(300);
			var success = await authPage.AttemptToAuthenticateAthlete();

			if (success && Navigation.ModalStack.Count > 0)
			{
				await Navigation.PopModalAsync();
				return true;
			}

			return false;
		}

		async protected void LogoutUser()
		{
			var decline = await DisplayAlert("Are you sure you want to log out of the mainframe?", null, "Yes", "No");

			if (!decline)
				return;

			var authViewModel = new AuthenticationViewModel();
			await authViewModel.LogOut(true);
			//App.Instance.StartRegistrationFlow();
		}

		#endregion
	}
}
