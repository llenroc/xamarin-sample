using System;


namespace Bullytect.Core.Config
{
    public static class SharedConfig
    {
        #if DEBUG
            public const string BASE_API_URL = "http://172.28.115.213:8080/api/v1";
#else
            public const string BASE_API_URL = "http://bulltec-dev.der.usal.es:8080/bullytect-integration-platform/api/v1";
#endif

        public const long TIMEOUT_OPERATION_MINUTES = 5;
        public const long TIMEOUT_OPERATIONS_SERVICES_SECOND = 30; 
		public const uint COMMON_ANIMATION_SPEED = 250;
		public const int COMMON_DELAY_SPEED = 300;

        public const string FACEBOOK_CLIENT_ID = "client_id";
	}
}
