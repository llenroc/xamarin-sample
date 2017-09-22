using System;
using System.Collections.Generic;

namespace Bullytect.Rest.Models.Exceptions
{
    public class DataInvalidException : Exception
    {

        public Dictionary<string, string> FieldErrors { get; set; }


    }
}
