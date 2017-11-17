

using System;
using System.Collections.Generic;
using Bullytect.Core.OAuth.Models;

namespace Bullytect.Core.OAuth.Services
{
    public interface IOAuthService
    {
        
        IObservable<Dictionary<string, string>> Authenticate(OAuth2 oauth2Info);
    }
}
