using System;
namespace Bullytect.Rest.Utils
{
    public static class ResponseNames
    {

        public const string RESPONSE_HEADER_NAME = "x-response-name";

        public const string VALIDATION_ERROR_RESPONSE = "VALIDATION_ERROR";
        public const string NO_ALERTS_FOUND_RESPONSE = "NO_ALERTS_FOUND";
        public const int NO_ALERTS_FOUND_RESPONSE_CODE = 701;
        public const string NO_CHILDREN_FOUND_FOR_SELF_PARENT_RESPONSE = "NO_CHILDREN_FOUND_FOR_SELF_PARENT";
        public const int NO_CHILDREN_FOUND_FOR_SELF_PARENT_CODE = 408;
        public const string PARENT_NOT_FOUND_RESPONSE = "PARENT_NOT_FOUND";
        public const int PARENT_NOT_FOUN_CODE = 406;
    }
}
