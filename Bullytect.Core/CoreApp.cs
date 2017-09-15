using System;
using System.Net.Http;
using Bullytect.Core.Config;
using Bullytect.Rest.Utils;
using Bullytect.Rest.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using AutoMapper;
using Bullytect.Rest.Models.Response;
using Bullytect.Core.Models.Domain;
using MvvmCross.Plugins.Validation;
using Bullytect.Rest.Utils.Logging;
using Acr.UserDialogs;
using Bullytect.Rest.Handlers;
using MvvmCross.Plugins.Messenger;
using Bullytect.Core.Messages;
using System.Diagnostics;

namespace Bullytect.Core
{
    public class CoreApp : MvvmCross.Core.ViewModels.MvxApplication
    {

        void prepareRestServices()
        {


            var httpClient = new HttpClient(new HttpLoggingHandler(
                new UnauthorizedHttpClientHandler(
                    () => {
                        Debug.WriteLine("Session Expired ....");
                        var messenger = Mvx.Resolve<IMvxMessenger>();
                        messenger.Publish(new SessionExpiredMessage(this));
                    }, new AuthenticatedHttpClientHandler(() => Settings.AccessToken))))
			{
				BaseAddress = new Uri(SharedConfig.BASE_API_URL),
				Timeout = TimeSpan.FromMinutes(SharedConfig.TIMEOUT_OPERATION_MINUTES)
			};

			// Register REST services
			Mvx.RegisterSingleton<IAuthenticationRestService>(() => RestServiceFactory.getService<IAuthenticationRestService>(httpClient));
			Mvx.RegisterSingleton<IParentsRestService>(() => RestServiceFactory.getService<IParentsRestService>(httpClient));
			Mvx.RegisterSingleton<IChildrenRestService>(() => RestServiceFactory.getService<IChildrenRestService>(httpClient));
            Mvx.RegisterSingleton<IDeviceGroupsRestService>(() => RestServiceFactory.getService<IDeviceGroupsRestService>(httpClient));
        }

        void prepareMappers() {

			Mapper.Initialize(cfg => {
				cfg.CreateMap<ParentDTO, ParentEntity>();
				cfg.CreateMap<SonDTO, SonEntity>();
                cfg.CreateMap<DeviceDTO, DeviceEntity>();
			});
        }


        public override void Initialize()
        {


            prepareRestServices();

            prepareMappers();

            CreatableTypes()
                .InNamespace("Bullytect.Core.Services")
                .EndingWith("ServiceImpl")
                .AsInterfaces()
                .RegisterAsLazySingleton();


            Mvx.RegisterType<IValidator, Validator>();

            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

            RegisterAppStart(new CustomAppStart());
			
        }
        
    }
}
