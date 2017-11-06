using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bullytect.Core.Rest.Models.Exceptions;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Services.Impl
{
    public class BaseRestServiceImpl
    {

        readonly HttpClient _client;

        public BaseRestServiceImpl(HttpClient client)
        {
            _client = client;

        }

		protected T DeserializeObject<T>(string value)
		{
			T result = default(T);
			try
			{
                result = JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });

            } catch (Exception ex) {
                Debug.WriteLine($"\nDeserialization failed with exception : { ex }\n");
            }

	        return result;
			
		}

        protected async Task<T> GetData<T>(string urlString)
		{
			var uri = new Uri(urlString);
			using (var resp = await _client.GetAsync(uri).ConfigureAwait(false))
			{
				if (!resp.IsSuccessStatusCode)
				{
                    throw await ApiException.Create(uri, HttpMethod.Get, resp).ConfigureAwait(false);
				}

				var content = await resp.Content.ReadAsStringAsync();
				return DeserializeObject<T>(content);
			}
        }

        protected async Task<T> GetData<T>(Uri uri)
		{
			using (var resp = await _client.GetAsync(uri).ConfigureAwait(false))
			{
				if (!resp.IsSuccessStatusCode)
				{
					throw await ApiException.Create(uri, HttpMethod.Get, resp).ConfigureAwait(false);
				}

				var content = await resp.Content.ReadAsStringAsync();
				return DeserializeObject<T>(content);
			}
		}

        protected async Task<T> PostData<T, E>(string urlString, E Data)
        {

            var uri = new Uri(urlString);
            var json = JsonConvert.SerializeObject(Data);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            using (var resp = await _client.PostAsync(uri, requestContent).ConfigureAwait(false))
            {
                if (!resp.IsSuccessStatusCode)
                {
                    throw await ApiException.Create(uri, HttpMethod.Post, resp).ConfigureAwait(false);
                }

                var responseContent = await resp.Content.ReadAsStringAsync();
                return DeserializeObject<T>(responseContent);
            }
        }


		protected async Task<T> PostStreamData<T>(string urlString, string Name, Stream Data)
		{

			var uri = new Uri(urlString);

			MultipartFormDataContent form = new MultipartFormDataContent();
			HttpContent content = new StringContent(Name);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
			form.Add(content, Name);
			content = new StreamContent(Data);
			content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
			{
				Name = Name,
				FileName = Name
			};
			form.Add(content);

			using (var resp = await _client.PostAsync(uri, form).ConfigureAwait(false))
			{
				if (!resp.IsSuccessStatusCode)
				{
					throw await ApiException.Create(uri, HttpMethod.Post, resp).ConfigureAwait(false);
				}

				var responseContent = await resp.Content.ReadAsStringAsync();
				return DeserializeObject<T>(responseContent);
			}
		}


		protected async Task<T> DeleteData<T>(string urlString)
		{

			var uri = new Uri(urlString);
			using (var resp = await _client.DeleteAsync(uri).ConfigureAwait(false))
			{
				if (!resp.IsSuccessStatusCode)
				{
                    throw await ApiException.Create(uri, HttpMethod.Delete, resp).ConfigureAwait(false);
				}

				var responseContent = await resp.Content.ReadAsStringAsync();
				return DeserializeObject<T>(responseContent);
			}
		}

    }
}
