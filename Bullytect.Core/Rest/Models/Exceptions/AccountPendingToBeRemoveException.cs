using System;
using Bullytect.Core.Rest.Models.Response;

namespace Bullytect.Core.Rest.Models.Exceptions
{
    public class AccountPendingToBeRemoveException: Exception
    {
        public APIResponse<string> Response { get; set; }
    }
}
