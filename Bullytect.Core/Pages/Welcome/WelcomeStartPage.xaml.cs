
using System;
using Bullytect.Core.ViewModels;
using MvvmCross.Forms.Core;

namespace Bullytect.Core.Pages.Welcome
{
    public partial class WelcomeStartPage : MvxContentPage<WelcomeViewModel>
	{
		public WelcomeStartPage()
		{
			InitializeComponent();
		}

		/*public WelcomeStartPage(bool isRuntime)
		{
			InitializeComponent();

			if (isRuntime)
			{
				label1.Scale = 0;
				label2.Scale = 0;
				buttonStack.Scale = 0;
			}

			await Task.Delay(SharedConfig.COMMON_DELAY_SPEED);
			await label1.ScaleTo(1, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
			await label2.ScaleTo(1, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
			await buttonStack.ScaleTo(1, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
		}*/

		async void AuthButtonClicked(object sender, EventArgs e)
		{
			

			/*if (App.Instance.CurrentAthlete != null)
            {
                await label1.FadeTo(0, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
                await label2.FadeTo(0, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
                await buttonStack.FadeTo(0, SharedConfig.COMMON_ANIMATION_SPEED, Easing.SinIn);
            }*/
		}

	}
}
