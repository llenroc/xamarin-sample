using System;
using System.Collections.Generic;
using System.IO;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Models.Request;

namespace Bullytect.Core.Rest.Services
{

    #pragma warning disable CS1701

    public interface IParentsRestService
    {

		IObservable<APIResponse<ParentDTO>> GetSelfInformation();

		IObservable<APIResponse<List<SonDTO>>> GetChildrenOfSelfParent();

		IObservable<APIResponse<ParentDTO>> registerParent(RegisterParentDTO parent);

		IObservable<APIResponse<ParentDTO>> updateSelfParent(UpdateParentDTO parent);

		IObservable<APIResponse<string>> resetPassword(ResetPasswordRequestDTO resetPasswordRequest);

		IObservable<APIResponse<string>> DeleteAccount();

		IObservable<APIResponse<ImageDTO>> UploadProfileImage(Stream stream);

		IObservable<APIResponse<SonDTO>> AddSonToSelfParent(RegisterSonDTO son);

		IObservable<APIResponse<SonDTO>> UpdateSonInformation(UpdateSonDTO son);

        IObservable<APIResponse<List<CommentsBySonDTO>>> GetCommentsBySonForLastIteration();

        IObservable<APIResponse<List<IterationDTO>>> GetLastIterations(int count);

        IObservable<APIResponse<UserSystemPreferencesDTO>> SavePreferences(SaveUserSystemPreferencesDTO preferences);

        IObservable<APIResponse<UserSystemPreferencesDTO>> GetPreferences();


    }
}
