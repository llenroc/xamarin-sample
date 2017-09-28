

namespace Bullytect.Rest.Services
{
	using System;
    using System.Collections.Generic;
    using Bullytect.Rest.Models.Request;
    using Bullytect.Rest.Models.Response;
	using Refit;

    #pragma warning disable CS1701

	[Headers("Accept: application/json")]
    public interface ISchoolRestService
    {
		[Get("/schools/all/names")]
        [Headers("Authorization: Bearer")]
        IObservable<APIResponse<IList<string>>> AllNames();

        [Post("/schools/")]
		[Headers("Authorization: Bearer")]
		IObservable<APIResponse<SchoolDTO>> CreateSchool([Body] AddSchoolDTO school);


    }
}
