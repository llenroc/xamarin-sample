
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
	using Bullytect.Rest.Services;
	using Refit;
    using System.Linq;

    public class ParentServiceImpl: BaseService, IParentService
    {
        readonly IParentsRestService _parentsRestService;

        public ParentServiceImpl(IParentsRestService parentsRestService)
        {
            _parentsRestService = parentsRestService;
        }

        public IObservable<ParentEntity> GetProfileInformation()
        {
           Debug.WriteLine("Get Profile Information");

            var observable =  _parentsRestService
                .GetSelfInformation()
                .Select(response => response.Data)
                .Select((ParentDTO parent) => Mapper.Map<ParentDTO, ParentEntity>(parent))
                .Finally(() => {
                    Debug.WriteLine("Get Profile Information finished ...");
                });

            return operationDecorator(observable);
        }

        public IObservable<List<SonEntity>> GetChildren()
        {
            
            Debug.WriteLine("Get Children");

            var observable =  _parentsRestService
                .GetChildrenOfSelfParent()
                .Select((APIResponse<List<SonDTO>> response) => response.Data)
                .Select((List<SonDTO> children) => Mapper.Map<List<SonDTO>, List<SonEntity>>(children))
				.Finally(() => {
					Debug.WriteLine("Get Children finished ...");
				});

            return operationDecorator(observable);
        }

        public IObservable<ParentEntity> Register(string FirstName, string LastName, DateTime Birthdate, 
                                                  string Email, string PasswordClear, string ConfirmPassword, string Telephone)
        {

            Debug.WriteLine("Register");

            var observable =  _parentsRestService
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

            return operationDecorator(observable);
        }

        public IObservable<ParentEntity> Update(string FirstName, string LastName, int Age, string Email)
        {
			Debug.WriteLine(String.Format("Update Parent with:t FirstName: {0}, LastName: {1}, Age: {2}, Email: {3}", FirstName, LastName, Age, Email));

            var observable =  _parentsRestService
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

            return operationDecorator(observable);
        }

        public IObservable<string> ResetPassword(string email)
        {
            Debug.WriteLine("Reset Password");

			var observable =  _parentsRestService
                .resetPassword(new ResetPasswordRequestDTO(){
                    Email = email
                })
				.Select((APIResponse<string> response) => response.Data)
				.Finally(() =>
				{
					Debug.WriteLine("Reset Password Finished ...");
				});


            return operationDecorator(observable);
        }
    }
}
