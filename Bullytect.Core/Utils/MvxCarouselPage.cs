

using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using Xamarin.Forms;

namespace Bullytect.Core.Utils
{
	public class MvxCarouselPage : CarouselPage, IMvxCarouselPage
	{


		public object DataContext
		{
			get
			{
				return BindingContext.DataContext;
			}
			set
			{
				base.BindingContext = value;
				BindingContext.DataContext = value;
			}
		}

		private IMvxBindingContext _bindingContext;
		public new IMvxBindingContext BindingContext
		{
			get
			{
				if (_bindingContext == null)
					BindingContext = new MvxBindingContext(base.BindingContext);
				return _bindingContext;
			}
			set
			{
				_bindingContext = value;
			}
		}

		public IMvxViewModel ViewModel
		{
			get { return DataContext as IMvxViewModel; }
			set { DataContext = value; }
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel?.ViewAppearing();
			ViewModel?.ViewAppeared();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ViewModel?.ViewDisappearing();
			ViewModel?.ViewDisappeared();
		}

		public MvxViewModelRequest Request { get; set; }

		
	}

	public class MvxCarouselPage<TViewModel>
		: MvxCarouselPage
	, IMvxCarouselPage<TViewModel> where TViewModel : class, IMvxViewModel
	{
		public new TViewModel ViewModel
		{
			get { return (TViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
	}
}
