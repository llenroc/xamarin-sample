using System;
using Bullytect.Rest.Models.Response;

namespace Bullytect.Rest.Models.Exceptions
{
    public class UploadImageFailException : Exception
    {
        public APIResponse<string> Response { get; set; }
    }
}
