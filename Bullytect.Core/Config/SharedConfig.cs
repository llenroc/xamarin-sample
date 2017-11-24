using System;


namespace Bullytect.Core.Config
{
    public static class SharedConfig
    {
#if DEBUG

         //public const string BASE_API_URL = "http://bulltec-dev.der.usal.es:8080/bulltect-integration-platform/api/v1";
        public const string BASE_API_URL = "http://192.168.0.106:8080/api/v1";

#else
            public const string BASE_API_URL = "http://bulltec-dev.der.usal.es:8080/bulltect-integration-platform/api/v1";
#endif

#if DEBUG

        public const string REDIRECT_URL = "http://192.168.0.105:8080/bulltect-integration-platform/backend/accounts/redirect";

#else
        public const string REDIRECT_URL = "http://bulltec-dev.der.usal.es:8080/bulltect-integration-platform/api/v1/children/redirect";

#endif

        public const long TIMEOUT_OPERATION_MINUTES = 5;
        public const long TIMEOUT_OPERATIONS_SERVICES_SECOND = 30; 
		public const uint COMMON_ANIMATION_SPEED = 250;
		public const int COMMON_DELAY_SPEED = 300;
        public const int COMMON_TOAST_DURATION = 8000;


        public const string FACEBOOK_CLIENT_ID = "341732922916068";
        public const string FACEBOOK_CLIENT_SECRET = "0c092e6878b000d856d5894ae0a49d14";

        public const string TRACKER_FOLDER_NAME = "bulltec_tracker";
	}
}
