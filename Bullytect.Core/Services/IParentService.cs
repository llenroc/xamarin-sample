using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface IParentService
    {

        Task<ParentEntity> getProfileInformation();
        Task<List<SonEntity>> getChildren();
        Task<ParentEntity> register(string FirstName, string LastName, int Age, string Email, string PasswordClear, string ConfirmPassword);
        Task<ParentEntity> update(string FirstName, string LastName, int Age, string Email);

    }
}
