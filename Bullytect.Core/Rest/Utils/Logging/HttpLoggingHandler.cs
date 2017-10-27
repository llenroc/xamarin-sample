using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bullytect.Utils.Helpers;

namespace Bullytect.Core.Rest.Utils.Logging
{
    #pragma warning disable CS1701

	public class HttpLoggingHandler : DelegatingHandler
	{
		public HttpLoggingHandler(HttpMessageHandler innerHandler = null)
			: base(innerHandler ?? new HttpClientHandler())
		{ }

        async protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)

        {
			var req = request;
			var id = Guid.NewGuid().ToString();

            StringBuilder Data = new StringBuilder();

			var msg = $"[{id} -   Request]";

            Data.Append($"{msg}========Start==========");
			Data.Append($"{msg} {req.Method} {req.RequestUri.PathAndQuery} {req.RequestUri.Scheme}/{req.Version}");
			Data.Append($"{msg} Host: {req.RequestUri.Scheme}://{req.RequestUri.Host}");

			foreach (var header in req.Headers)
				Data.Append($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

			if (req.Content != null)
			{
				foreach (var header in req.Content.Headers)
					Data.Append($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

				if (req.Content is StringContent || this.IsTextBasedContentType(req.Headers) || this.IsTextBasedContentType(req.Content.Headers))
				{
					var result = await req.Content.ReadAsStringAsync();

					Data.Append($"{msg} Content:");
					Data.Append($"{msg} {string.Join("", result.Cast<char>().Take(255))}...");

				}
			}

			var start = DateTime.Now;

			var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

			var end = DateTime.Now;

			Data.Append($"{msg} Duration: {end - start}");
			Data.Append($"{msg}==========End==========");

			msg = $"[{id} - Response]";
			Data.Append($"{msg}=========Start=========");

			var resp = response;

			Data.Append($"{msg} {req.RequestUri.Scheme.ToUpper()}/{resp.Version} {(int)resp.StatusCode} {resp.ReasonPhrase}");

			foreach (var header in resp.Headers)
				Data.Append($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

			if (resp.Content != null)
			{
				foreach (var header in resp.Content.Headers)
					Data.Append($"{msg} {header.Key}: {string.Join(", ", header.Value)}");

				if (resp.Content is StringContent || this.IsTextBasedContentType(resp.Headers) || this.IsTextBasedContentType(resp.Content.Headers))
				{
					start = DateTime.Now;
					var result = await resp.Content.ReadAsStringAsync();
					end = DateTime.Now;

					Data.Append($"{msg} Content:");
					Data.Append($"{msg} {string.Join("", result.Cast<char>().Take(255))}...");
					Data.Append($"{msg} Duration: {end - start}");
				}
			}

			Data.Append($"{msg}==========End==========");

            Data.TrackToConsole();
            Data.TrackToFile();
			return response;
		}

		readonly string[] types = new[] { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

		bool IsTextBasedContentType(HttpHeaders headers)
		{
			IEnumerable<string> values;
			if (!headers.TryGetValues("Content-Type", out values))
				return false;
			var header = string.Join(" ", values).ToLowerInvariant();

			return types.Any(t => header.Contains(t));
		}
	}
}
