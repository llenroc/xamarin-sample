
namespace Bullytect.Core.Services
{

    using System;
    using Bullytect.Rest.Models.Exceptions;

    public interface IAuthenticationService
    {
        IObservable<string> LogIn(string email, string password, Action<AuthenticationFailedException> errorHandler);
        bool IsLoggedIn();
    }
}
