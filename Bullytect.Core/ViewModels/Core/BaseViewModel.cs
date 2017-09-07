

namespace Bullytect.Core.ViewModels
{

	using System;
	using Bullytect.Core.Models.Utils;
	using MvvmCross.ReactiveUI.Interop;


    public class BaseViewModel: MvxReactiveViewModel, IDirty
    {
       
        bool _isBusy;

        public event EventHandler IsBusyChanged;

		public bool IsBusy
		{
			get
			{
				return _isBusy;
			}
			set
			{
                if (SetProperty(ref _isBusy, value))
				{
					if (IsBusyChanged != null)
						IsBusyChanged(this, new EventArgs());
				}
			}
		}

		public bool IsDirty
		{
			get;
			set;
		}
    }

	public class Busy : IDisposable
	{
		readonly object _sync = new object();
		readonly BaseViewModel _viewModel;

		public Busy(BaseViewModel viewModel)
		{
			_viewModel = viewModel;
			lock (_sync)
			{
				_viewModel.IsBusy = true;
			}
		}

		public void Dispose()
		{
			lock (_sync)
			{
				_viewModel.IsBusy = false;
			}
		}
	}
}
