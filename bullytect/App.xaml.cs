using System;
using System.Net.Http;
using Autofac;
using Autofac.Core;
using bullytect.config;
using bullytect.rest.services;
using bullytect.rest.utils;
using Xamarin.Forms;

namespace bullytect
{
    public partial class App : Application
    {
        private static IContainer _container;

		

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
			containerBuilder.Register(c => RestServiceFactory.getService<IAuthenticationService>(httpClient));
			// Register Parents Service
			containerBuilder.Register(c => RestServiceFactory.getService<IParentsService>(httpClient));
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
            PrepareContainer(platformSpecificModules);
            InitializeComponent();
            MainPage = new bullytectPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
