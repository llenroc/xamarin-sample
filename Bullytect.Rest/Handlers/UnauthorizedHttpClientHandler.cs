

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bullytect.Rest.Handlers
{
    #pragma warning disable CS1701

	public class UnauthorizedHttpClientHandler: DelegatingHandler
    {

        readonly Action _onUnauthorizedError;
		public UnauthorizedHttpClientHandler(Action onUnauthorizedError, HttpMessageHandler innerHandler = null)
            : base(innerHandler ?? new HttpClientHandler())
        {
            _onUnauthorizedError = onUnauthorizedError;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
       
           var response =  await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                _onUnauthorizedError();

            return response;

        }
    }
}
