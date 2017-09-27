
namespace Bullytect.Core.Services
{

	using System;
	using System.Collections.Generic;
    using System.IO;
    using Bullytect.Core.Models.Domain;
	using Bullytect.Rest.Models.Exceptions;


    public interface IParentService
    {

        IObservable<ParentEntity> GetProfileInformation();
        IObservable<List<SonEntity>> GetChildren();
		IObservable<ParentEntity> Register(string FirstName, string LastName, DateTime Birthdate,
												  string Email, string PasswordClear, string ConfirmPassword, string Telephone);
        IObservable<ParentEntity> Update(string FirstName, string LastName, string Birthdate, string Email, string Telephone);
        IObservable<string> ResetPassword(string email);
        IObservable<string> DeleteAccount();
        IObservable<ImageEntity> UploadProfileImage(Stream FileStream);
        IObservable<SonEntity> AddSonToSelfParent(string FirstName, string Lastname, DateTime Birthdate);
        IObservable<SonEntity> GetSonById(string Id);
    }
}
