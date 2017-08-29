using System;
namespace bullytect.config
{
    public static class SharedConfig
    {
        #if DEBUG
            public const string BASE_API_URL = "http://192.168.0.200:8080/bullytect-integration-platform-0.0.6-SNAPSHOT/";
#else
            public const string BASE_API_URL = "http://192.168.0.200:8080/bullytect-integration-platform-0.0.6-SNAPSHOT/";
#endif

        public const uint COMMON_ANIMATION_SPEED = 250;
		public const int COMMON_DELAY_SPEED = 300;
	}
}
