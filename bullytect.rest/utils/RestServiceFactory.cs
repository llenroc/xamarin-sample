using System;
using System.Net.Http;
using Refit;

namespace Bullytect.Rest.Utils
{
    public static class RestServiceFactory
    {
        #pragma warning disable CS1701

		public static T getService<T>(HttpClient client)
		{
            return RestService.For<T>(client);
        }

    }
}
