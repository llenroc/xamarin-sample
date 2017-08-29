using System;
using System.Threading.Tasks;
using bullytect.config;
using bullytect.ViewModels;
using Xamarin.Forms;

namespace bullytect.Pages.Welcome
{
	public partial class WelcomeStartPage : WelcomeStartPageXaml
	{
		public WelcomeStartPage()
		{
			Initialize();
		}

		public WelcomeStartPage(bool isRuntime)
		{
			Initialize();

			if (isRuntime)
			{
				label1.Scale = 0;
				label2.Scale = 0;
				buttonStack.Scale = 0;
			}
		}

		protected override void Initialize()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			InitializeComponent();
			Title = "Welcome!";
		}

		async void AuthButtonClicked(object sender, EventArgs e)
		{
			await ViewModel.Authenticate();

			/*if (App.Instance.CurrentAthlete != null)
			{
				await label1.FadeTo(0, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
				await label2.FadeTo(0, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
				await buttonStack.FadeTo(0, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
			}*/
		}

		protected async override void OnLoaded()
		{
			base.OnLoaded();

			await Task.Delay(SharedConfig.COMMON_DELAY_SPEED);
			await label1.ScaleTo(1, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
			await label2.ScaleTo(1, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
			await buttonStack.ScaleTo(1, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
		}
	}

	public partial class WelcomeStartPageXaml : BaseContentPage<AuthenticationViewModel>
	{
	}
}
