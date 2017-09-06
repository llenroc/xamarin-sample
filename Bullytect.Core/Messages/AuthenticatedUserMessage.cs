using System;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.Messages
{
    public class AuthenticatedUserMessage : MvxMessage
    {
        public AuthenticatedUserMessage(object sender) : base(sender)
        {
        }
    }
}
