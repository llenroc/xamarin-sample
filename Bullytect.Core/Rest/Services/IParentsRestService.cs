using System;
using System.Collections.Generic;
using System.IO;
using Refit;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Models.Request;

namespace Bullytect.Core.Rest.Services
{

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface IParentsRestService
    {

		[Get("/parents/self")]
        [Headers("Authorization: Bearer")]
		IObservable<APIResponse<ParentDTO>> GetSelfInformation();

		[Get("/parents/self/children")]
        [Headers("Authorization: Bearer")]
		IObservable<APIResponse<List<SonDTO>>> GetChildrenOfSelfParent();

		[Post("/parents/")]
		IObservable<APIResponse<ParentDTO>> registerParent([Body] RegisterParentDTO parent);

        [Post("/parents/self")]
        [Headers("Authorization: Bearer")]
        IObservable<APIResponse<ParentDTO>> updateSelfParent([Body] UpdateParentDTO parent);

		[Post("/parents/reset-password")]
		[Headers("Authorization: Bearer")]
		IObservable<APIResponse<string>> resetPassword([Body] ResetPasswordRequestDTO resetPasswordRequest);

		[Delete("/parents/self/delete")]
		[Headers("Authorization: Bearer")]
		IObservable<APIResponse<string>> DeleteAccount();

		[Multipart]
		[Post("/parents/self/image")]
		[Headers("Authorization: Bearer")]
		IObservable<APIResponse<ImageDTO>> UploadProfileImage([AttachmentName("profile_image")] Stream stream);

		[Post("/parents/self/children/add")]
		[Headers("Authorization: Bearer")]
		IObservable<APIResponse<SonDTO>> AddSonToSelfParent([Body] RegisterSonDTO son);

		[Post("/parents/self/children/update")]
		[Headers("Authorization: Bearer")]
        IObservable<APIResponse<SonDTO>> UpdateSonInformation([Body] UpdateSonDTO son);


    }
}
