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
using Microcharts;
using static Bullytect.Core.Rest.Models.Response.SentimentAnalysisStatisticsDTO;
using SkiaSharp;
using static Bullytect.Core.Rest.Models.Response.DimensionsStatisticsDTO;
using static Bullytect.Core.Rest.Models.Response.CommunitiesStatisticsDTO;
using static Bullytect.Core.Rest.Models.Response.SocialMediaActivityStatisticsDTO;
using static Bullytect.Core.Rest.Models.Response.CommentsAnalyzedStatisticsDTO;
using static Bullytect.Core.Rest.Models.Response.SocialMediaLikesStatisticsDTO;
using static Bullytect.Core.Rest.Models.Response.AlertsStatisticsDTO;

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

        void prepareCommonMappers(IMapperConfigurationExpression cfg) {

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
			cfg.CreateMap<IterationDTO, IterationEntity>();
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

        }


        void prepareMappersForCharts(IMapperConfigurationExpression cfg){

			// Sentiment Analysis Chart

			//Mapper for SentimentDTO to Microchart Entry
			cfg.CreateMap<SentimentDTO, Entry>()
				.ConstructUsing(s => new Entry(s.Score))
				.ForMember(s => s.Label, (obj) => obj.ResolveUsing(o => o.Type))
				.ForMember(s => s.Value, (obj) => obj.ResolveUsing(o => o.Score))
				.ForMember(s => s.ValueLabel, (obj) => obj.ResolveUsing(o => o.Label))
				.ForMember(s => s.Color, (obj) => obj.ResolveUsing(o =>
				{
					SkiaSharp.SKColor Color;
					SentimentEnum sentiment = o.Type.ToEnum<SentimentEnum>();
					if (sentiment.Equals(SentimentEnum.POSITIVE))
					{
						Color = SKColor.Parse("#00FF00");
					}
					else if (sentiment.Equals(SentimentEnum.NEGATIVE))
					{
						Color = SKColor.Parse("#FF0000");
					}
					else
					{
						Color = SKColor.Parse("#9c9c9c");
					}
					return Color;

				}));

			//Mapper for SentimentAnalysisStatisticsDTO to ChartModel
			cfg.CreateMap<SentimentAnalysisStatisticsDTO, ChartModel>()
				.ForMember(s => s.Entries, (obj) => obj.MapFrom((o) => o.SentimentData))
				.ForMember(s => s.Type, (obj) => obj.UseValue(typeof(DonutChart)));


			// Dimensions Chart

			//Mapper for DimensionDTO to Microchart Entry
			cfg.CreateMap<DimensionDTO, Entry>()
				.ConstructUsing(s => new Entry(s.Value))
				.ForMember(s => s.Label, (obj) => obj.ResolveUsing(o => o.Type))
				.ForMember(s => s.Value, (obj) => obj.ResolveUsing(o => o.Value))
				.ForMember(s => s.ValueLabel, (obj) => obj.ResolveUsing(o => o.Label))
				.ForMember(s => s.Color, (obj) => obj.UseValue(SKColor.Parse("#6BC7E0")));

			//Mapper for DimensionsStatisticsDTO to ChartModel
			cfg.CreateMap<DimensionsStatisticsDTO, ChartModel>()
				.ForMember(s => s.Entries, (obj) => obj.MapFrom((o) => o.Dimensions))
				.ForMember(s => s.Type, (obj) => obj.UseValue(typeof(BarChart)));


			// Communities Chart

			//Mapper for CommunityDTO to Microchart Entry
			cfg.CreateMap<CommunityDTO, Entry>()
				.ConstructUsing(s => new Entry(s.Value))
				.ForMember(s => s.Label, (obj) => obj.ResolveUsing(o => o.Type))
				.ForMember(s => s.Value, (obj) => obj.ResolveUsing(o => o.Value))
				.ForMember(s => s.ValueLabel, (obj) => obj.ResolveUsing(o => o.Label))
				.ForMember(s => s.Color, (obj) => obj.UseValue(SKColor.Parse("#6BC7E0")));

			//Mapper for CommunitiesStatisticsDTO to ChartModel
			cfg.CreateMap<CommunitiesStatisticsDTO, ChartModel>()
				.ForMember(s => s.Entries, (obj) => obj.MapFrom((o) => o.Communities))
				.ForMember(s => s.Type, (obj) => obj.UseValue(typeof(LineChart)));

			// Social Media Activities Chart

			//Mapper for ActivityDTO to Microchart Entry
			cfg.CreateMap<ActivityDTO, Entry>()
				.ConstructUsing(s => new Entry(s.Value))
				.ForMember(s => s.Label, (obj) => obj.ResolveUsing(o => o.Type))
				.ForMember(s => s.Value, (obj) => obj.ResolveUsing(o => o.Value))
				.ForMember(s => s.ValueLabel, (obj) => obj.ResolveUsing(o => o.Label))
				.ForMember(s => s.Color, (obj) => obj.ResolveUsing(o => {
					SkiaSharp.SKColor Color;
					SocialMediaTypeEnum socialMedia = o.Type.ToEnum<SocialMediaTypeEnum>();
					if (socialMedia.Equals(SocialMediaTypeEnum.FACEBOOK))
					{
						Color = SKColor.Parse("#3b5998");
					}
					else if (socialMedia.Equals(SocialMediaTypeEnum.INSTAGRAM))
					{
						Color = SKColor.Parse("#E03867");
					}
					else
					{
						Color = SKColor.Parse("#cc181e");
					}
					return Color;
				}));

			//Mapper for SocialMediaActivityStatisticsDTO to ChartModel
			cfg.CreateMap<SocialMediaActivityStatisticsDTO, ChartModel>()
				.ForMember(s => s.Entries, (obj) => obj.MapFrom((o) => o.Activities))
				.ForMember(s => s.Type, (obj) => obj.UseValue(typeof(DonutChart)));


			// Comments Analyzed Chart

			//Mapper for CommentAnalyzedDTO  to Microchart Entry
			cfg.CreateMap<CommentAnalyzedDTO, Entry>()
				.ConstructUsing(s => new Entry(s.Total))
				.ForMember(s => s.Label, (obj) => obj.ResolveUsing(o => String.Format("{0:d/M/yyyy HH:mm:ss}", o.Date)))
				.ForMember(s => s.Value, (obj) => obj.ResolveUsing(o => o.Total))
				.ForMember(s => s.ValueLabel, (obj) => obj.ResolveUsing(o => o.Label))
				.ForMember(s => s.Color, (obj) => obj.UseValue(SKColor.Parse("#6BC7E0")));

			//Mapper for CommentsAnalyzedStatisticsDTO to ChartModel
			cfg.CreateMap<CommentsAnalyzedStatisticsDTO, ChartModel>()
				.ForMember(s => s.Entries, (obj) => obj.MapFrom((o) => o.Data))
				.ForMember(s => s.Type, (obj) => obj.UseValue(typeof(LineChart)));


			// Social Media Likes Chart

			//Mapper for SocialMediaLikesDTO  to Microchart Entry
			cfg.CreateMap<SocialMediaLikesDTO, Entry>()
                .ConstructUsing(s => new Entry(s.Likes))
                .ForMember(s => s.Label, (obj) => obj.ResolveUsing(o => o.Type))
                .ForMember(s => s.Value, (obj) => obj.ResolveUsing(o => o.Likes))
                .ForMember(s => s.ValueLabel, (obj) => obj.ResolveUsing(o => o.Label))
				.ForMember(s => s.Color, (obj) => obj.ResolveUsing(o => {
					SkiaSharp.SKColor Color;
					SocialMediaTypeEnum socialMedia = o.Type.ToEnum<SocialMediaTypeEnum>();
					if (socialMedia.Equals(SocialMediaTypeEnum.FACEBOOK))
					{
						Color = SKColor.Parse("#3b5998");
					}
					else if (socialMedia.Equals(SocialMediaTypeEnum.INSTAGRAM))
					{
						Color = SKColor.Parse("#E03867");
					}
					else
					{
						Color = SKColor.Parse("#cc181e");
					}
					return Color;
				}));

			//Mapper for SocialMediaLikesStatisticsDTO to ChartModel
			cfg.CreateMap<SocialMediaLikesStatisticsDTO, ChartModel>()
				.ForMember(s => s.Entries, (obj) => obj.MapFrom((o) => o.Data))
                .ForMember(s => s.Type, (obj) => obj.UseValue(typeof(BarChart)));

			// Alerts Statistics Chart

			//Mapper for AlertLevelDTO  to Microchart Entry
			cfg.CreateMap<AlertLevelDTO, Entry>()
                .ConstructUsing(s => new Entry(s.Total))
                .ForMember(s => s.Label, (obj) => obj.ResolveUsing(o => o.Level))
				.ForMember(s => s.Value, (obj) => obj.ResolveUsing(o => o.Total))
				.ForMember(s => s.ValueLabel, (obj) => obj.ResolveUsing(o => o.Label))
				.ForMember(s => s.Color, (obj) => obj.ResolveUsing(o => {
					SkiaSharp.SKColor Color;
                    AlertLevelEnum alertLevel = o.Level.ToEnum<AlertLevelEnum>();
                    switch(alertLevel) {
                        case AlertLevelEnum.INFO:
                            Color = SKColor.Parse("#2C8DA9");
                            break;
                        case AlertLevelEnum.WARNING:
                            Color = SKColor.Parse("#FFA700");
                            break;
                        case AlertLevelEnum.DANGER:
                            Color = SKColor.Parse("#D93028");
                            break;
                        case AlertLevelEnum.SUCCESS:
							Color = SKColor.Parse("#E03867");
							break;
                    }
					return Color;
				}));

			//Mapper for AlertsStatisticsDTO to ChartModel
			cfg.CreateMap<AlertsStatisticsDTO, ChartModel>()
				.ForMember(s => s.Entries, (obj) => obj.MapFrom((o) => o.Data))
                .ForMember(s => s.Type, (obj) => obj.UseValue(typeof(DonutChart)));


		}

        void prepareMappers() {
            
			Mapper.Initialize(cfg => {
                prepareCommonMappers(cfg);
                prepareMappersForCharts(cfg);
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
