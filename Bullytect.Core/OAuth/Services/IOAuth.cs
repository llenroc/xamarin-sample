

using System;
using Bullytect.Core.OAuth.Models;

namespace Bullytect.Core.OAuth.Services
{
    public interface IOAuth
    {
        IObservable<string> authenticate(OAuth2 oauth2Info);
    }
}
