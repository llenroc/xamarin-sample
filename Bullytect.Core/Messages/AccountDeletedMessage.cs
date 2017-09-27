using System;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.Messages
{
    public class SignOutMessage : MvxMessage
    {

        public SignOutMessage(object sender) : base(sender)
        {
        }
    }
}
