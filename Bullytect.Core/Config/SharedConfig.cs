using System;


namespace Bullytect.Core.Config
{
    public static class SharedConfig
    {
        #if DEBUG
            public const string BASE_API_URL = "http://192.168.0.200:8080/bullytect-integration-platform-0.0.9-SNAPSHOT/api/v1";
#else
            public const string BASE_API_URL = "http://192.168.0.200:8080/bullytect-integration-platform-0.0.9-SNAPSHOT/api/v1";
#endif

		public const long TIMEOUT_OPERATION_MINUTES = 5;

		public const uint COMMON_ANIMATION_SPEED = 250;
		public const int COMMON_DELAY_SPEED = 300;
	}
}
