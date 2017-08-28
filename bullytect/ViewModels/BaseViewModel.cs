using System;
using bullytect.Models.Utils;

namespace bullytect.ViewModels
{
    public class BaseViewModel: BaseNotify, IDirty
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
				if (SetPropertyChanged(ref _isBusy, value))
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
}
