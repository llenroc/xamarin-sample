using System;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Request
{
    public class ResetPasswordRequestDTO
    {

        #pragma warning disable CS1701

		[JsonProperty("email")]
        public string Email { get; set; }
    }
}
