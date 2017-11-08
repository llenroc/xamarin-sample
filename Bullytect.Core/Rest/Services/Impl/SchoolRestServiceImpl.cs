using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Request;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Rest.Services.Impl
{
    public class SchoolRestServiceImpl: BaseRestServiceImpl, ISchoolRestService
    {

        public SchoolRestServiceImpl(HttpClient client): base(client)
        {
        }

        public IObservable<APIResponse<IList<SchoolNameDTO>>> AllNames()
        {
            return Observable.FromAsync(() => GetData<APIResponse<IList<SchoolNameDTO>>>(ApiEndpoints.ALL_SCHOOL_NAMES));
        }

        public IObservable<APIResponse<SchoolDTO>> CreateSchool(AddSchoolDTO school)
        {
            return Observable.FromAsync(() => PostData<APIResponse<SchoolDTO>, AddSchoolDTO>(ApiEndpoints.CREATE_SCHOOL, school));
        }

        public IObservable<APIResponse<IList<SchoolDTO>>> FindSchools(string name)
        {
            return Observable.FromAsync(() => GetData<APIResponse<IList<SchoolDTO>>>(new Uri(ApiEndpoints.FIND_SCHOOLS).AttachParameters(new Dictionary<string, string>()
            {
                { "name", name }
            })));
        }

        public IObservable<APIResponse<string>> Total()
        {
            return Observable.FromAsync(() => GetData<APIResponse<string>>(ApiEndpoints.COUNT_SCHOOL));
        }
    }
}
