using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace Bullytect.Core.Utils
{
	public interface IMvxCarouselPage : IMvxView, IMvxBindingContextOwner
	{
		MvxViewModelRequest Request { get; set; }
	}

	public interface IMvxCarouselPage<TViewModel>
		: IMvxCarouselPage , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
	{
	}
}
