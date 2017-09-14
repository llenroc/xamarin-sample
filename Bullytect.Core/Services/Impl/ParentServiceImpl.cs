
namespace Bullytect.Core.Services.Impl
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reactive.Linq;
	using AutoMapper;
	using Bullytect.Core.Models.Domain;
	using Bullytect.Rest.Models.Request;
	using Bullytect.Rest.Models.Response;
	using Bullytect.Rest.Models.Exceptions;
	using Bullytect.Rest.Services;
	using Refit;

	public class ParentServiceImpl: IParentService
    {
        readonly IParentsRestService _parentsRestService;

        public ParentServiceImpl(IParentsRestService parentsRestService)
        {
            _parentsRestService = parentsRestService;
        }

        public IObservable<ParentEntity> getProfileInformation(Action<LoadProfileFailedException> errorHandler)
        {
           Debug.WriteLine("Get Profile Information");

            return _parentsRestService
                .GetSelfInformation()
                .Select(response => response.Data)
                .Select((ParentDTO parent) => Mapper.Map<ParentDTO, ParentEntity>(parent))
                .Catch<ParentEntity, ApiException>(ex => {
                    var response = ex.GetContentAs<APIResponse<string>>();
                    errorHandler(new LoadProfileFailedException(response));
                    return null;
                }).Finally(() => {
                    Debug.WriteLine("Get Profile Information finished ...");
                });
        }

        public IObservable<List<SonEntity>> getChildren()
        {
            
            Debug.WriteLine("Get Children");

            return _parentsRestService
                .GetChildrenOfSelfParent()
                .Select((APIResponse<List<SonDTO>> response) => response.Data)
                .Select((List<SonDTO> children) => Mapper.Map<List<SonDTO>, List<SonEntity>>(children))
				.Finally(() => {
					Debug.WriteLine("Get Children finished ...");
				});
        }

        public IObservable<ParentEntity> register(string FirstName, string LastName, DateTime Birthdate, 
                                                  string Email, string PasswordClear, string ConfirmPassword, string Telephone)
        {

            Debug.WriteLine("Register");

            return _parentsRestService
                .registerParent(new RegisterParentDTO()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Birthdate = Birthdate,
                    Email = Email,
                    PasswordClear = PasswordClear,
                    ConfirmPassword = ConfirmPassword,
                    Telephone = Telephone
                })
                .Select((APIResponse<ParentDTO> response) => response.Data)
                .Select(parent => Mapper.Map<ParentDTO, ParentEntity>(parent))
                .Finally(() =>
                {
                    Debug.WriteLine("Register ...");
                });
        }

        public IObservable<ParentEntity> update(string FirstName, string LastName, int Age, string Email)
        {
			Debug.WriteLine(String.Format("Update Parent with:t FirstName: {0}, LastName: {1}, Age: {2}, Email: {3}", FirstName, LastName, Age, Email));

            return _parentsRestService
                .updateSelfParent(new UpdateParentDTO()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Age = Age,
                    Email = Email
                })
                .Select((APIResponse<ParentDTO> response) => response.Data)
                .Select(parent => Mapper.Map<ParentDTO, ParentEntity>(parent))
				.Finally(() =>
				{
					Debug.WriteLine("Update ...");
				});
        }

    }
}
