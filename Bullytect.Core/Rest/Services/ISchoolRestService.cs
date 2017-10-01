

namespace Bullytect.Core.Rest.Services
{
	using System;
    using System.Collections.Generic;
	using Refit;
    using Bullytect.Core.Rest.Models.Response;
    using Bullytect.Core.Rest.Models.Request;

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
