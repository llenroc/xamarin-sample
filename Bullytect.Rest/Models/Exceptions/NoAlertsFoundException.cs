using System;
using Bullytect.Rest.Models.Response;

namespace Bullytect.Rest.Models.Exceptions
{
    public class NoAlertsFoundException: Exception
    {
        public APIResponse<string> Response { get; set; }
    }
}
