using System;
using Bullytect.Rest.Models.Response;

namespace Bullytect.Rest.Models.Exceptions
{
    public class NoChildrenFoundException: Exception
    {
        public APIResponse<string> Response { get; set; }
    }
}
