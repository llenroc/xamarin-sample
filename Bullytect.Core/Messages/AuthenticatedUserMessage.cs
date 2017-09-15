using System;
using MvvmCross.Plugins.Messenger;

namespace Bullytect.Core.Messages
{
    public class AuthenticatedUserMessage : MvxMessage
    {
        
        public string JwtToken { get; set; }

        public AuthenticatedUserMessage(object sender) : base(sender)
        {
        }
    }
}
