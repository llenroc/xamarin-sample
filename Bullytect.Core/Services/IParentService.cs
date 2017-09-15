
namespace Bullytect.Core.Services
{

	using System;
	using System.Collections.Generic;
	using Bullytect.Core.Models.Domain;
	using Bullytect.Rest.Models.Exceptions;


    public interface IParentService
    {

        IObservable<ParentEntity> GetProfileInformation(Action<LoadProfileFailedException> errorHandler);
        IObservable<List<SonEntity>> GetChildren();
		IObservable<ParentEntity> Register(string FirstName, string LastName, DateTime Birthdate,
												  string Email, string PasswordClear, string ConfirmPassword, string Telephone);
        IObservable<ParentEntity> Update(string FirstName, string LastName, int Age, string Email);
    }
}
