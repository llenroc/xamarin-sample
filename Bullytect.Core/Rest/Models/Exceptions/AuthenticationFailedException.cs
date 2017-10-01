using Bullytect.Core.Rest.Models.Response;
using System;

namespace Bullytect.Core.Rest.Models.Exceptions
{
    public class AuthenticationFailedException : Exception
    {

        readonly APIResponse<string> _response;

        public AuthenticationFailedException(APIResponse<string> response)
        {
            _response = response;
        }


        public APIResponse<string> Response { get; private set; }

    }
}
