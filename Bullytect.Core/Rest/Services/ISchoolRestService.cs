

namespace Bullytect.Core.Rest.Services
{
	using System;
    using System.Collections.Generic;
    using Bullytect.Core.Rest.Models.Response;
    using Bullytect.Core.Rest.Models.Request;

#pragma warning disable CS1701

    public interface ISchoolRestService
    {
		// /schools/all/names
		IObservable<APIResponse<IList<SchoolNameDTO>>> AllNames();

		// /schools/
		IObservable<APIResponse<SchoolDTO>> CreateSchool(AddSchoolDTO school);


    }
}
