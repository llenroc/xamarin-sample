using System;
using Bullytect.Rest.Models.Response;

namespace Bullytect.Rest.Models.Exceptions
{
    public class LoadProfileFailedException : Exception
    {

        readonly APIResponse<string> _response;

        public LoadProfileFailedException(APIResponse<string> response)
        {
            _response = response;
        }


        public APIResponse<string> Response { get; private set; }

    }
}
