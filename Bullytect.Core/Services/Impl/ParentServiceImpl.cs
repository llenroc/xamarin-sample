
namespace Bullytect.Core.Services.Impl
{

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using AutoMapper;
    using Bullytect.Core.Models.Domain;
    using System.Linq;
    using MvvmCross.Plugins.Messenger;
    using Bullytect.Core.Messages;
    using System.IO;
    using Bullytect.Core.Rest.Services;
    using Bullytect.Core.Rest.Models.Response;
    using Bullytect.Core.Rest.Models.Request;
    using Bullytect.Core.Config;

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

        public IObservable<ParentEntity> Update(string FirstName, string LastName, DateTime Birthdate, string Email, string Telephone)
        {
            Debug.WriteLine(String.Format("Update Parent with:t FirstName: {0}, LastName: {1}, Birthdate: {2}, Email: {3}, Telephone: {4}", FirstName, LastName, Birthdate, Email, Telephone));

            var observable =  _parentsRestService
                .updateSelfParent(new UpdateParentDTO()
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Birthdate = Birthdate,
                    Email = Email,
                    Telephone = Telephone ?? string.Empty
                })
                .Select((APIResponse<ParentDTO> response) => response.Data)
                .Select(parent => Mapper.Map<ParentDTO, ParentEntity>(parent))
				.Finally(() =>
				{
					Debug.WriteLine("Update Profile Finished ...");
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

        public IObservable<SonEntity> AddSonToSelfParent(string FirstName, string Lastname, DateTime Birthdate, string School)
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

        public IObservable<SonEntity> UpdateSonInformation(string Identity, string FirstName, string Lastname, DateTime Birthdate, string School)
        {
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

        public IObservable<ImageEntity> UploadSonProfileImage(string identity, Stream stream)
        {
			Debug.WriteLine("Upload Son Profile Image");

            var observable = _childrenRestService
                .UploadProfileImage(identity, stream)
				.Select((response) => response.Data)
				.Select(image => Mapper.Map<ImageDTO, ImageEntity>(image))
				.Finally(() =>
				{
					Debug.WriteLine("Upload Son Profile Image Finished ...");
				});

			return operationDecorator(observable);
        }

        public IObservable<Dictionary<string, string>> GetCommentsBySonForLastIteration()
        {

            var commentsBySon = new Dictionary<string, string>() {

                { "Sergio Sánchez", "30" },
                { "David Martín", "20" }

            };


            return Observable.Return(commentsBySon);



        }

        public IObservable<List<IterationEntity>> GetLastIterations()
        {

            Debug.WriteLine("Get Last Iterations ...");

            var observable = _parentsRestService
                .GetLastIterations(Settings.Current.IterationsCountToShow)
                .Select((response) => response.Data)
                .Select(iterations => Mapper.Map<List<IterationDTO>, List<IterationEntity>>(iterations))
				.Finally(() =>
				{
					Debug.WriteLine("Get Last Finished ...");
				});

			return operationDecorator(observable);
        }
    }
}
