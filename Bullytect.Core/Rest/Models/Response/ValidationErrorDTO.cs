
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bullytect.Core.Rest.Models.Response
{
    public class ValidationErrorDTO
    {
        #pragma warning disable CS1701

		
        List<FieldErrorDTO> _fieldErrors = new List<FieldErrorDTO>();

        [JsonProperty("field_errors")]
        public List<FieldErrorDTO> FieldErrors { get; set; }

	}
}
