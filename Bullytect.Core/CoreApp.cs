using System;
using System.Net.Http;
using Bullytect.Core.Config;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using AutoMapper;
using Bullytect.Core.Models.Domain;
using MvvmCross.Plugins.Validation;
using Acr.UserDialogs;
using MvvmCross.Plugins.Messenger;
using Bullytect.Core.Messages;
using System.Diagnostics;
using Bullytect.Core.Rest.Utils.Logging;
using Bullytect.Core.Rest.Handlers;
using Bullytect.Core.Rest.Services;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Services.Impl;

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
			Mvx.RegisterSingleton<IAuthenticationRestService>(() => new AuthenticationRestServiceImpl(httpClient));
			Mvx.RegisterSingleton<IParentsRestService>(() => new ParentsRestServiceImpl(httpClient));
			Mvx.RegisterSingleton<IChildrenRestService>(() => new ChildrenRestServiceImpl(httpClient));
            Mvx.RegisterSingleton<IDeviceGroupsRestService>(() => new DeviceGroupsRestServiceImpl(httpClient));
            Mvx.RegisterSingleton<IAlertRestService>(() => new AlertRestServiceImpl(httpClient));
            Mvx.RegisterSingleton<ISchoolRestService>(() => new SchoolRestServiceImpl(httpClient));
        }

        void prepareMappers() {

			Mapper.Initialize(cfg => {
				cfg.CreateMap<ParentDTO, ParentEntity>();
				cfg.CreateMap<SonDTO, SonEntity>();
                cfg.CreateMap<DeviceDTO, DeviceEntity>();
                cfg.CreateMap<AlertDTO, AlertEntity>();
                cfg.CreateMap<ImageDTO, ImageEntity>();
                cfg.CreateMap<SocialMediaDTO, SocialMediaEntity>();
                cfg.CreateMap<SchoolDTO, SchoolEntity>();
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
