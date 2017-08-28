using System;
using System.Net.Http;
using Autofac;
using Autofac.Core;
using bullytect.config;
using bullytect.rest.services;
using bullytect.rest.utils;
using Bullytect.Utils.Helpers;
using Xamarin.Forms;

namespace bullytect
{
    public partial class App : Application
    {

        private static IContainer _container;

        static App _instance;

		public static App Instance
		{
			get
			{
				return _instance;
			}
		}

		private static void RegisterPlatformSpecificModules(IModule[] platformSpecificModules, ContainerBuilder containerBuilder)
		{
			foreach (var platformSpecificModule in platformSpecificModules)
			{
				containerBuilder.RegisterModule(platformSpecificModule);
			}
		}

		private static void RegisterRestServices(ContainerBuilder containerBuilder)
		{

			var httpClient = new HttpClient()
			{
				BaseAddress = new Uri(SharedConfig.BASE_API_URL)
			};
			// Register Authentication Service.
            containerBuilder.Register(c => RestServiceFactory.getService<IAuthenticationService>(httpClient)).InstancePerDependency();
			// Register Parents Service
            containerBuilder.Register(c => RestServiceFactory.getService<IParentsService>(httpClient)).InstancePerDependency();
            // Register Children Service
            containerBuilder.Register(c => RestServiceFactory.getService<IChildrenService>(httpClient)).InstancePerDependency();

		}

		private static void PrepareContainer(IModule[] platformSpecificModules)
		{
			var containerBuilder = new Autofac.ContainerBuilder();
			// Register Paltform Modules
			RegisterPlatformSpecificModules(platformSpecificModules, containerBuilder);
			// Register REST Services.
			RegisterRestServices(containerBuilder);
			// Build Container.
			_container = containerBuilder.Build();
		}


        public App(IModule[] platformSpecificModules)
        {
            _instance = this;
            PrepareContainer(platformSpecificModules);
            InitializeComponent();
            MainPage = new bullytectPage();
        }

        protected override void OnStart()
        {
            // Handling for App Exceptions.
            MessagingCenter.Subscribe<object, Exception>(this, EventTypeName.EXCEPTION_OCCURRED, (object sender, Exception exception) => {
				if (exception == null)
					return;
				exception.Track();
            });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
