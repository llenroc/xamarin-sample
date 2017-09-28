

namespace Bullytect.Core.Services.Impl
{

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reactive.Linq;
	using AutoMapper;
	using Bullytect.Core.Models.Domain;
	using Bullytect.Rest.Models.Response;
	using Bullytect.Rest.Services;

    public class SocialMediaServiceImpl: BaseService, ISocialMediaService
    {

        readonly IChildrenRestService _childrenRestService;

        public SocialMediaServiceImpl(IChildrenRestService childrenRestService) {
            _childrenRestService = childrenRestService;
        }

        public IObservable<SocialMediaEntity> DeleteSocialMedia(string IdSon, string IdSocial)
        {
            Debug.WriteLine(string.Format("Delete Social Media for: Id Son {0}, Id Social: {1}"));

            var observable = _childrenRestService
                .DeleteSocialMedia(IdSon, IdSocial)
				.Select(response => response.Data)
				.Select(socialMedia => Mapper.Map<SocialMediaDTO, SocialMediaEntity>(socialMedia))
				.Finally(() => {
					Debug.WriteLine("Delete Social Media Finished ...");
				});

            return operationDecorator(observable);
        }

        public IObservable<IList<SocialMediaEntity>> GetAllSocialMediaBySon(string Id)
        {
            Debug.WriteLine(string.Format("Get all social media for son with id: {0}", Id));

            var observable = _childrenRestService
                .GetAllSocialMediaBySonId(Id)
                .Select(response => response.Data)
                .Select(socialMedias => Mapper.Map<IList<SocialMediaDTO>, IList<SocialMediaEntity>>(socialMedias))
				.Finally(() => {
					Debug.WriteLine("Get All Social Media By Son  Finished ...");
				});

            return operationDecorator(observable);

		}

        public IObservable<IList<SocialMediaEntity>> SaveAllSocialMedia(string IdSon, IList<SocialMediaEntity> SocialMediaEntities)
        {
			Debug.WriteLine("Save All Social Medias");

            var observable = _childrenRestService
                .SaveAllSocialMedia(IdSon, Mapper.Map<IList<SocialMediaEntity>, IList<SocialMediaDTO>>(SocialMediaEntities))
                .Select(Response => Response.Data)
                .Select(SocialMedias => Mapper.Map<IList<SocialMediaDTO>, IList<SocialMediaEntity>>(SocialMedias))
				.Finally(() => {
					Debug.WriteLine("Save Social Media Finished ...");
				});

            return operationDecorator(observable);
        }

        public IObservable<SocialMediaEntity> SaveSocialMedia(string AccessToken, string Type, string Son)
        {

            Debug.WriteLine(string.Format("Save Social Media with AccessToken: {0}, Type: {1}, Son: {2}"));

            var observable = _childrenRestService
                .SaveSocialMedia(new Rest.Models.Request.SaveSocialMediaDTO(){
                    AccessToken = AccessToken,
                    Type = Type,
                    Son = Son
			    })
                .Select(response => response.Data)
                .Select(socialMedias => Mapper.Map<SocialMediaDTO, SocialMediaEntity>(socialMedias))
				.Finally(() => {
					Debug.WriteLine("Save Social Media Finished ...");
				});

            return operationDecorator(observable);
        }
    }
}
