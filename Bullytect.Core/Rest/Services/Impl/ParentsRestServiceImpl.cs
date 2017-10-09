using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Linq;
using Bullytect.Core.Rest.Models.Request;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Utils;

namespace Bullytect.Core.Rest.Services.Impl
{
    public class ParentsRestServiceImpl: BaseRestServiceImpl,  IParentsRestService
    {

        public ParentsRestServiceImpl(HttpClient client): base(client){}

        public IObservable<APIResponse<SonDTO>> AddSonToSelfParent(RegisterSonDTO son)
        {
            return Observable.FromAsync(() => PostData<APIResponse<SonDTO>, RegisterSonDTO>(ApiEndpoints.ADD_SON_TO_SELF_PARENT, son));
        }

        public IObservable<APIResponse<string>> DeleteAccount()
        {
            return Observable.FromAsync(() => DeleteData<APIResponse<string>>(ApiEndpoints.DELETE_ACCOUNT));
        }

        public IObservable<APIResponse<List<SonDTO>>> GetChildrenOfSelfParent()
        {
            return Observable.FromAsync(() => GetData<APIResponse<List<SonDTO>>>(ApiEndpoints.GET_CHILDREN_OF_SELF_PARENT));
        }

        public IObservable<APIResponse<List<CommentsBySonDTO>>> GetCommentsBySonForLastIteration()
        {
            return Observable.FromAsync(() => GetData<APIResponse<List<CommentsBySonDTO>>>(ApiEndpoints.GET_COMMENTS_BY_SON_FOR_LAST_ITERATION));
        }

        public IObservable<APIResponse<List<IterationDTO>>> GetLastIterations(int count)
        {

			return Observable.FromAsync(() => GetData<APIResponse<List<IterationDTO>>>(new Uri(ApiEndpoints.GET_LAST_ITERATIONS_FOR_SELF_PARENT).AttachParameters(new Dictionary<string, string>()
			{
                { "count", count.ToString()}
			})));
        }

        public IObservable<APIResponse<ParentDTO>> GetSelfInformation()
        {
            return Observable.FromAsync(() => GetData<APIResponse<ParentDTO>>(ApiEndpoints.GET_SELF_PARENT_INFORMATION));
        }

        public IObservable<APIResponse<ParentDTO>> registerParent(RegisterParentDTO parent)
        {
            return Observable.FromAsync(() => PostData<APIResponse<ParentDTO>, RegisterParentDTO>(ApiEndpoints.REGISTER_PARENT, parent));
        }

        public IObservable<APIResponse<string>> resetPassword(ResetPasswordRequestDTO resetPasswordRequest)
        {
            return Observable.FromAsync(() => PostData<APIResponse<string>, ResetPasswordRequestDTO>(ApiEndpoints.RESET_PASSWORD, resetPasswordRequest));
        }

        public IObservable<APIResponse<ParentDTO>> updateSelfParent(UpdateParentDTO parent)
        {
            return Observable.FromAsync(() => PostData<APIResponse<ParentDTO>, UpdateParentDTO>(ApiEndpoints.UPDATE_SELF_PARENT, parent));
        }

        public IObservable<APIResponse<SonDTO>> UpdateSonInformation(UpdateSonDTO son)
        {
            return Observable.FromAsync(() => PostData<APIResponse<SonDTO>, UpdateSonDTO>(ApiEndpoints.UPDATE_SON_INFORMATION, son));
        }

        public IObservable<APIResponse<ImageDTO>> UploadProfileImage(Stream stream)
        {
            return Observable.FromAsync(() => PostStreamData<APIResponse<ImageDTO>>(ApiEndpoints.UPLOAD_PROFILE_IMAGE, "profile_image", stream));
        }
    }
}
