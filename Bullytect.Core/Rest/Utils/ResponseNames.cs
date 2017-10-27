

namespace Bullytect.Core.Rest.Utils
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
        public const string GENERIC_ERROR_RESPONSE = "GENERIC_ERROR";
        public const int GENERIC_ERROR_CODE= 502;
        public const string FAILED_TO_UPLOAD_IMAGE_RESPONSE = "FAILED_TO_UPLOAD_IMAGE";
        public const int FAILED_TO_UPLOAD_IMAGE_CODE = 1001;
        public const string NO_SCHOOLS_FOUND_RESPONSE = "NO_SCHOOLS_FOUND";
        public const int NO_SCHOOLS_FOUND_CODE = 606;
        public const string BAD_CREDENTIALS_RESPONSE = "BAD_CREDENTIALS";
        public const int BAD_CREDENTIALS_CODE = 416;
		public const string ACCOUNT_DISABLED_RESPONSE = "ACCOUNT_DISABLED";
		public const int ACCOUNT_DISABLED_CODE = 417;
		public const string ACCOUNT_LOCKED_RESPONSE = "ACCOUNT_LOCKED";
		public const int ACCOUNT_LOCKED_CODE = 505;
		public const string NO_NEW_ALERTS_FOUND_RESPONSE = "NO_NEW_ALERTS_FOUND";
		public const int NO_NEW_ALERTS_FOUND_CODE = 709;
        public const string NO_ALERTS_BY_SON_FOUNDED_RESPONSE = "NO_ALERTS_BY_SON_FOUNDED";
        public const int NO_ALERTS_BY_SON_FOUNDED_CODE = 107;
        public const string SOCIAL_MEDIA_BY_CHILD_NOT_FOUND_RESPONSE = "SOCIAL_MEDIA_BY_CHILD_NOT_FOUND";
        public const int SOCIAL_MEDIA_BY_CHILD_NOT_FOUND_CODE = 302;
        public const string NO_ITERATIONS_FOUND_FOR_SELF_PARENT_RESPONSE = "NO_ITERATIONS_FOUND_FOR_SELF_PARENT";
        public const int NO_ITERATIONS_FOUND_FOR_SELF_PARENT_CODE = 430;
        public const string EMAIL_ALREADY_EXISTS_RESPONSE = "EMAIL_ALREADY_EXISTS";
        public const int EMAIL_ALREADY_EXISTS_CODE = 437;
	}
}
