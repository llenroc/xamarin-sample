using System;
using System.Net.Http;
using Bullytect.Core.Config;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using AutoMapper;
using Bullytect.Core.Models.Domain;
using MvvmCross.Plugins.Validation;
using Acr.UserDialogs;
using System.Diagnostics;
using Bullytect.Core.Rest.Utils.Logging;
using Bullytect.Core.Rest.Handlers;
using Bullytect.Core.Rest.Services;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Services.Impl;
using FFImageLoading;
using FFImageLoading.Config;
using Bullytect.Core.Models.Domain.Converter;
using MvvmCross.Core.Navigation;
using Bullytect.Core.ViewModels;
using Bullytect.Utils.Helpers;
using Bullytect.Core.Helpers;
using Bullytect.Core.OAuth.Services.Impl;
using Bullytect.Core.OAuth.Services;

namespace Bullytect.Core
{
    public class CoreApp : MvvmCross.Core.ViewModels.MvxApplication
    {

        void prepareRestServices(HttpClient httpClient)
        {
 
			Mvx.RegisterSingleton<IAuthenticationRestService>(() => new AuthenticationRestServiceImpl(httpClient));
			Mvx.RegisterSingleton<IParentsRestService>(() => new ParentsRestServiceImpl(httpClient));
			Mvx.RegisterSingleton<IChildrenRestService>(() => new ChildrenRestServiceImpl(httpClient));
            Mvx.RegisterSingleton<IDeviceGroupsRestService>(() => new DeviceGroupsRestServiceImpl(httpClient));
            Mvx.RegisterSingleton<IAlertRestService>(() => new AlertRestServiceImpl(httpClient));
            Mvx.RegisterSingleton<ISchoolRestService>(() => new SchoolRestServiceImpl(httpClient));

        }

        void prepareMappers() {
            
			Mapper.Initialize(cfg => {
                cfg.CreateMap<string, DateTime>().ConvertUsing<StringToDateTimeConverter>();
				cfg.CreateMap<ParentDTO, ParentEntity>();
				cfg.CreateMap<SonDTO, SonEntity>();
                cfg.CreateMap<DeviceDTO, DeviceEntity>();
                cfg.CreateMap<AlertDTO, AlertEntity>()
                    .ForMember(d => d.Level, (obj) => 
                               obj.ResolveUsing(o => o?.Level.ToEnum<AlertLevelEnum>()));
                cfg.CreateMap<ImageDTO, ImageEntity>();
                cfg.CreateMap<SocialMediaDTO, SocialMediaEntity>();
                cfg.CreateMap<SchoolDTO, SchoolEntity>();
                cfg.CreateMap<AlertsPageDTO, AlertsPageEntity>();
			});
        }


        public override void Initialize()
        {

			var httpClient = new HttpClient(new HttpLoggingHandler(
				new UnauthorizedHttpClientHandler(
					() => {
						Debug.WriteLine("Session Expired ....");
						var navigationService = Mvx.Resolve<IMvxNavigationService>();
						Config.Settings.AccessToken = null;
						navigationService?.Navigate<AuthenticationViewModel>();

					}, new AuthenticatedHttpClientHandler(() => Config.Settings.AccessToken))))
			{
				BaseAddress = new Uri(SharedConfig.BASE_API_URL),
				Timeout = TimeSpan.FromMinutes(SharedConfig.TIMEOUT_OPERATION_MINUTES)
			};


            prepareRestServices(httpClient);

            prepareMappers();

            CreatableTypes()
                .InNamespace("Bullytect.Core.Services")
                .EndingWith("ServiceImpl")
                .AsInterfaces()
                .RegisterAsLazySingleton();


            Mvx.RegisterType<IValidator, Validator>();

            Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

            // Register App Helper
            Mvx.LazyConstructAndRegisterSingleton<AppHelper, AppHelper>();
            Mvx.LazyConstructAndRegisterSingleton<IOAuthService, IOAuthServiceImpl>();

			ImageService.Instance.Initialize(new Configuration
			{
				HttpClient = httpClient
			});

            RegisterAppStart(new CustomAppStart());
			
        }
        
    }
}
