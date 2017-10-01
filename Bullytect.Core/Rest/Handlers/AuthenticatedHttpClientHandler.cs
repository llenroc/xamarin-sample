using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Bullytect.Core.Rest.Handlers
{
    public class AuthenticatedHttpClientHandler: DelegatingHandler
    {

        #pragma warning disable CS170

		readonly Func<string> getToken;

		public AuthenticatedHttpClientHandler(Func<string> getToken, HttpMessageHandler innerHandler = null)
            : base(innerHandler ?? new HttpClientHandler())
        {
			if (getToken == null) throw new ArgumentNullException("getToken");
			this.getToken = getToken;
		}


		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			// See if the request has an authorize header
			var auth = request.Headers.Authorization;

			if (auth != null)
			{
                Debug.WriteLine(String.Format("Schema : {0}, Parameter: {1}", auth.Scheme, auth.Parameter));
                var token = getToken();
                if( token != null ) {
					Debug.WriteLine(String.Format("Token : {0}", token));
					request.Headers.Authorization = new AuthenticationHeaderValue(token);
                }

			}

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
