using System;
using System.Net.Http;
using Bullytect.Core.Config;
using Bullytect.Core.Rest.Handlers;
using Bullytect.Core.Rest.Utils.Logging;

namespace Bullytect.Core.Rest.Utils
{
    public class HttpClientFactory
    {
        private static HttpClient httpClient = null;


        public static HttpClient getHttpClient(){

            if(httpClient == null) {

				httpClient = new HttpClient(new HttpLoggingHandler(
				new UnauthorizedHttpClientHandler(new AuthenticatedHttpClientHandler(() => Config.Settings.AccessToken))))
				{
					BaseAddress = new Uri(SharedConfig.BASE_API_URL),
					Timeout = TimeSpan.FromMinutes(SharedConfig.TIMEOUT_OPERATION_MINUTES)
				};

				httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
				httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            }

            return httpClient;

        }
    }
}
