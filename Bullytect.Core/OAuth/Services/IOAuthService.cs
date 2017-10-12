

using System;
using System.Threading.Tasks;
using Bullytect.Core.OAuth.Models;

namespace Bullytect.Core.OAuth.Services
{
    public interface IOAuthService
    {
        IObservable<string> Authenticate(OAuth2 oauth2Info);
    }
}
