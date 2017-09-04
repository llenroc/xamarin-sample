using System;
using System.Net.Http;
using Bullytect.Core.Config;
using Bullytect.Rest.Utils;
using Bullytect.Rest.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using Bullytect.Core.ViewModels;

namespace Bullytect.Core
{
    public class CoreApp : MvvmCross.Core.ViewModels.MvxApplication
    {

        public override void Initialize()
        {
            
			var httpClient = new HttpClient()
			{
				BaseAddress = new Uri(SharedConfig.BASE_API_URL)
			};


            // Register REST services
            Mvx.RegisterSingleton<IAuthenticationRestService>(() => RestServiceFactory.getService<IAuthenticationRestService>(httpClient));
            Mvx.RegisterSingleton<IParentsRestService>(() => RestServiceFactory.getService<IParentsRestService>(httpClient));
            Mvx.RegisterSingleton<IChildrenRestService>(() => RestServiceFactory.getService<IChildrenRestService>(httpClient));

            CreatableTypes()
                .InNamespace("Bullytect.Core.Services")
                .EndingWith("ServiceImpl")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<AuthenticationViewModel>();
			
        }

        
    }
}
