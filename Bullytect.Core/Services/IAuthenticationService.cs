
namespace Bullytect.Core.Services
{

    using System;

    public interface IAuthenticationService
    {
        IObservable<string> LogIn(string email, string password);
        IObservable<string> LoginWithFacebook(string accessToken);
        IObservable<string> LoginWithGoogle(string accessToken);
        bool IsLoggedIn();
    }
}
