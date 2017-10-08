using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class SchoolNameDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("identity")]
		public string Identity { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
    }
}
