using System;
using Bullytect.Rest.Models.Response;

namespace Bullytect.Rest.Models.Exceptions
{
    public class LoadProfileFailedException : Exception
    {

        public APIResponse<string> Response { get; set; }

    }
}
