using System;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.Messages
{
    public class ExceptionOcurredMessage: MvxMessage
    {

		public ExceptionOcurredMessage(object sender, Exception ex) : base(sender)
        {
            Ex = ex;
		}

		public Exception Ex
		{
			get;
			private set;
		}
       
    }
}
