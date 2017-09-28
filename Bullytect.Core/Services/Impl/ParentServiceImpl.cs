
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
    using System.Linq;
    using MvvmCross.Plugins.Messenger;
    using Bullytect.Core.Messages;
    using System.IO;

    public class ParentServiceImpl: BaseService, IParentService
    {
        readonly IParentsRestService _parentsRestService;
        readonly IMvxMessenger _mvxMessenger;
        readonly IChildrenRestService _childrenRestService;

        public ParentServiceImpl(IParentsRestService parentsRestService, IMvxMessenger mvxMessenger, IChildrenRestService childrenRestService)
        {
            _parentsRestService = parentsRestService;
            _mvxMessenger = mvxMessenger;
            _childrenRestService = childrenRestService;
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

        public IObservable<ParentEntity> Update(string FirstName, string LastName, string Birthdate, string Email, string Telephone)
        {
            Debug.WriteLine(String.Format("Update Parent with:t FirstName: {0}, LastName: {1}, Birthdate: {2}, Email: {3}, Telephone: {4}", FirstName, LastName, Birthdate, Email, Telephone));

            var observable =  _parentsRestService
                .updateSelfParent(new UpdateParentDTO()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Birthdate = Birthdate,
                    Email = Email,
                    Telephone = Telephone
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

        public IObservable<string> DeleteAccount()
        {
            Debug.WriteLine("Delete Account");

            var observable = _parentsRestService
                .DeleteAccount()
                .Select((APIResponse<string> response) => response.Data)
                .Do((_) => _mvxMessenger.Publish(new AccountDeletedMessage(this){}))
                .Finally(() =>
                {
                    Debug.WriteLine("Delete Account Finished ...");
                });

            return operationDecorator(observable);
        }

        public IObservable<ImageEntity> UploadProfileImage(Stream FileStream)
        {
            Debug.WriteLine("Upload Profile Image");

            var observable = _parentsRestService
                .UploadProfileImage(FileStream)
                .Select((response) => response.Data)
                .Select(image => Mapper.Map<ImageDTO, ImageEntity>(image))
				.Finally(() =>
				{
					Debug.WriteLine("Upload Profile Image Finished ...");
				});

            return operationDecorator(observable);

        }

        public IObservable<SonEntity> GetSonById(string Id)
        {

            Debug.WriteLine(string.Format("Get Son With id: {0}", Id));

            var observable = _childrenRestService
                .GetSonById(Id)
                .Select((response) => response.Data)
                .Select(son => Mapper.Map<SonDTO, SonEntity>(son))
				.Finally(() =>
				{
					Debug.WriteLine("Get Son By Id ....");
				});

            return operationDecorator(observable);
            
        }

        public IObservable<SonEntity> AddSonToSelfParent(string FirstName, string Lastname, string Birthdate, string School)
        {
            Debug.WriteLine("Add Son To Self Parent");

			var observable = _parentsRestService
                .AddSonToSelfParent(new RegisterSonDTO()
				{
					FirstName = FirstName,
					LastName = Lastname,
					Birthdate = Birthdate,
					School = School
				}).Select((response) => response.Data)
				.Select(son => Mapper.Map<SonDTO, SonEntity>(son))
				.Finally(() =>
				{
					Debug.WriteLine("Add Son To Self Parent Finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<SonEntity> UpdateSonInformation(string Identity, string FirstName, string Lastname, string Birthdate, string School)
        {
			Debug.WriteLine("Save Son Information");

			var observable = _parentsRestService
                .UpdateSonInformation(new UpdateSonDTO()
				{
                    Identity = Identity,
					FirstName = FirstName,
					LastName = Lastname,
					Birthdate = Birthdate,
					School = School
				}).Select((response) => response.Data)
				.Select(son => Mapper.Map<SonDTO, SonEntity>(son))
				.Finally(() =>
				{
					Debug.WriteLine("Save Son Information Finished ...");
				});

			return operationDecorator(observable);
        }
    }
}
