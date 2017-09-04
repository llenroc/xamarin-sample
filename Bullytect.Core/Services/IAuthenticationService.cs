
namespace Bullytect.Core.Services
{

    using System;
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        Task<string> LogIn(string email, string password);
    }
}
