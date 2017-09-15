using System;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.Messages
{
    public class SessionExpiredMessage: MvxMessage
    {
		public SessionExpiredMessage(object sender) : base(sender)
        {
		}
    }
}
