using System;
using Xamarin.Forms;

namespace Bullytect.Core.Controls
{
	public class AnimateButton : Button
	{
		public AnimateButton() : base()
		{
			const int _animationTime = 100;
			Clicked += async (sender, e) =>
			{
				var btn = (AnimateButton)sender;
				await btn.ScaleTo(1.2, _animationTime);
				btn.ScaleTo(1, _animationTime);
			};
		}
	}
}
