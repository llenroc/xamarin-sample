using System;
using System.Collections.Generic;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface ISocialMediaService
    {

        IObservable<IList<SocialMediaEntity>> GetAllSocialMediaBySon(string Id);
        IObservable<SocialMediaEntity> SaveSocialMedia(string AccessToken, string Type, string Son);
        IObservable<IList<SocialMediaEntity>> SaveAllSocialMedia(string IdSon, IList<SocialMediaEntity> SocialMediaEntities);
        IObservable<SocialMediaEntity> DeleteSocialMedia(string IdSon, string IdSocial);


    }
}
