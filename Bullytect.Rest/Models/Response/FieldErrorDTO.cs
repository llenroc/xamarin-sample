using System;
using Newtonsoft.Json;

namespace Bullytect.Rest.Models.Response
{
    public class FieldErrorDTO
    {
        #pragma warning disable CS1701

		[JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
