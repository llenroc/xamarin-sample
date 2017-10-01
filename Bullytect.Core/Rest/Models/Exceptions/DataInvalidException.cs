using System;
using System.Collections.Generic;

namespace Bullytect.Core.Rest.Models.Exceptions
{
    public class DataInvalidException : Exception
    {

        public Dictionary<string, string> FieldErrors { get; set; }


    }
}
