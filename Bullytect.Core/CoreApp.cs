using System;
using System.Net.Http;
using Bullytect.Core.Config;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using AutoMapper;
using Bullytect.Core.Models.Domain;
using Acr.UserDialogs;
using Bullytect.Core.Rest.Utils.Logging;
using Bullytect.Core.Rest.Handlers;
using Bullytect.Core.Rest.Services;
using Bullytect.Core.Rest.Models.Response;
using Bullytect.Core.Rest.Services.Impl;
using FFImageLoading;
using FFImageLoading.Config;
using Bullytect.Core.Models.Domain.Converter;
using Bullytect.Utils.Helpers;
using Bullytect.Core.Helpers;
using Bullytect.Core.OAuth.Services.Impl;
using Bullytect.Core.OAuth.Services;
using Bullytect.Core.Rest.Models.Request;
using Bullytect.Core.ViewModels.Core.Models;

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
				cfg.CreateMap<SonDTO, SonEntity>()
                .ForMember(s => s.SchoolIdentity, (obj) => 
                           obj.ResolveUsing(o => o?.School.Identity))
                .ForMember(s => s.SchoolName, (obj) =>
                           obj.ResolveUsing(o => o?.School.Name));
                cfg.CreateMap<DeviceDTO, DeviceEntity>();
                cfg.CreateMap<AlertDTO, AlertEntity>()
                    .ForMember(d => d.Level, (obj) => 
                               obj.ResolveUsing(o => o?.Level.ToEnum<AlertLevelEnum>()));
                cfg.CreateMap<ImageDTO, ImageEntity>();
                cfg.CreateMap<SocialMediaDTO, SocialMediaEntity>();
				cfg.CreateMap<SocialMediaEntity, SaveSocialMediaDTO>();
                cfg.CreateMap<SchoolDTO, SchoolEntity>();
                cfg.CreateMap<AlertsPageDTO, AlertsPageEntity>();

                // Mapper for SonEntity to SonCategoryModel
                cfg.CreateMap<SonEntity, SonCategoryModel>()
                .ForMember(s => s.Name, (obj) =>
                           obj.MapFrom((Son) => Son.FullName))
                .ForMember(s => s.Description, (obj) =>
                           obj.MapFrom((Son) => Son.FullName))
                .ForMember(s => s.IsFiltered, (obj) =>
                           obj.ResolveUsing(o => Settings.Current.ShowResultsForAllChildren || Settings.Current.FilteredSonCategories.Contains(o.Identity)))
                .ForMember(s => s.IsEnabled, (obj) =>
                           obj.UseValue<bool>(!Settings.Current.ShowResultsForAllChildren));
                    

			});
        }


        public override void Initialize()
        {

			var httpClient = new HttpClient(new HttpLoggingHandler(
				new UnauthorizedHttpClientHandler(new AuthenticatedHttpClientHandler(() => Config.Settings.AccessToken))))
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
