﻿using Bullytect.Core.Rest.Models.Response;
using System;

namespace Bullytect.Core.Rest.Models.Exceptions
{
    public class NoDimensionsStatisticsForThisPeriodException : Exception
    {
        public APIResponse<string> Response { get; set; }
    }
}
