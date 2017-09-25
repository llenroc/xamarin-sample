using System;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.Messages
{
    public class AccountDeletedMessage : MvxMessage
    {

        public AccountDeletedMessage(object sender) : base(sender)
        {
        }
    }
}
