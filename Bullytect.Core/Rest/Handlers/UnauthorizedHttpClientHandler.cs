

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bullytect.Core.Rest.Models.Exceptions;

namespace Bullytect.Core.Rest.Handlers
{
    #pragma warning disable CS1701

	public class UnauthorizedHttpClientHandler: DelegatingHandler
    {

        readonly Action _onUnauthorizedError;
		public UnauthorizedHttpClientHandler(HttpMessageHandler innerHandler = null)
            : base(innerHandler ?? new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
       
            var response =  await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            Debug.WriteLine("Check if authorized ..."+ response.StatusCode);
            if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                throw new ApiUnauthorizedAccessException();

            return response;

        }
    }
}
