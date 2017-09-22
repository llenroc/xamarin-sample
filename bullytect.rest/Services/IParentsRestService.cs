using System;
using System.Collections.Generic;
using Bullytect.Rest.Models.Request;
using Bullytect.Rest.Models.Response;
using Refit;

namespace Bullytect.Rest.Services
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


    }
}
