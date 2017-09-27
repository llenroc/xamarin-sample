using System;
using System.Collections.Generic;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.Services
{
    public interface ISocialMediaService
    {

        IObservable<IList<SocialMediaEntity>> GetAllSocialMediaBySon(string Id);


    }
}
