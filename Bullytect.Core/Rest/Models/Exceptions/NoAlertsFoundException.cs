using Bullytect.Core.Rest.Models.Response;
using System;

namespace Bullytect.Core.Rest.Models.Exceptions
{
    public class NoAlertsFoundException: Exception
    {
        public APIResponse<string> Response { get; set; }
    }
}
