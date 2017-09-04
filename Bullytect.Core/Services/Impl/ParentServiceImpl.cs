using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Bullytect.Core.Models.Domain;
using Bullytect.Rest.Models.Request;
using Bullytect.Rest.Models.Response;
using Bullytect.Rest.Services;

namespace Bullytect.Core.Services.Impl
{
    public class ParentServiceImpl: IParentService
    {
        readonly IParentsRestService _parentsRestService;

        public ParentServiceImpl(IParentsRestService parentsRestService)
        {
            _parentsRestService = parentsRestService;
        }

        public Task<ParentEntity> getProfileInformation()
        {
           Debug.WriteLine("Get Profile Information");

            return _parentsRestService.GetSelfInformation()
                                      .ContinueWith(t => t.Result.Data, TaskContinuationOptions.OnlyOnRanToCompletion)
                                      .ContinueWith(t => Mapper.Map<ParentDTO, ParentEntity>(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }


		public Task<List<SonEntity>> getChildren()
		{
			Debug.WriteLine("Get Children");

            return _parentsRestService.GetChildrenOfSelfParent()
                                      .ContinueWith(t => t.Result.Data, TaskContinuationOptions.OnlyOnRanToCompletion)
                                      .ContinueWith(t => Mapper.Map<List<SonDTO>, List<SonEntity>>(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
		}

        public Task<ParentEntity> register(string FirstName, string LastName, int Age, string Email, string PasswordClear, string ConfirmPassword)
        {
			Debug.WriteLine("Register Parent");

            return _parentsRestService.registerParent(new RegisterParentDTO()
            {
                FirstName = FirstName,
                LastName = LastName,
                Age = Age,
                Email = Email,
                PasswordClear = PasswordClear,
                ConfirmPassword = ConfirmPassword
            })
                                      .ContinueWith(t => t.Result.Data, TaskContinuationOptions.OnlyOnRanToCompletion)
                                      .ContinueWith(t => Mapper.Map<ParentDTO, ParentEntity>(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }


        public Task<ParentEntity> update(string FirstName, string LastName, int Age, string Email)
        {
            Debug.WriteLine(String.Format("Update Parent with:t FirstName: {0}, LastName: {1}, Age: {2}, Email: {3}", FirstName, LastName, Age, Email));

            return _parentsRestService.updateSelfParent(new UpdateParentDTO()
            {
                FirstName = FirstName,
                LastName = LastName,
                Age = Age,
                Email = Email
            }).ContinueWith(t => t.Result.Data, TaskContinuationOptions.OnlyOnRanToCompletion)
                                      .ContinueWith(t => Mapper.Map<ParentDTO, ParentEntity>(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}
